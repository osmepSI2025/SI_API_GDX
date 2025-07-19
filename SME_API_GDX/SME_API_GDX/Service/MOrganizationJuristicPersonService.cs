using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SME_API_GDX.Entities;
using SME_API_GDX.Models;
using SME_API_GDX.Repository;
using SME_API_GDX.Services;
using System.Text.Json;

public class MOrganizationJuristicPersonService 
{
    private readonly MOrganizationJuristicPersonRepository _repository;
    private readonly ICallAPIService _serviceApi;
    private readonly IApiInformationRepository _repositoryApi;
    private readonly string _FlagDev;

    public MOrganizationJuristicPersonService(MOrganizationJuristicPersonRepository repository, IConfiguration configuration, ICallAPIService serviceApi, IApiInformationRepository repositoryApi)

    {
        _repository = repository;
        _serviceApi = serviceApi;
        _repositoryApi = repositoryApi;
        _FlagDev = configuration["Devlopment:FlagDev"] ?? throw new ArgumentNullException("FlagDev is missing in appsettings.json");

    }

    public Task<IEnumerable<MOrganizationJuristicPerson>> GetAllAsync()
        => _repository.GetAllAsync();

    public Task<MOrganizationJuristicPerson?> GetByIdAsync(string id)
        => _repository.GetByIdAsync(id);

    public Task<bool> AddAsync(MOrganizationJuristicPerson entity)
        => _repository.AddAsync(entity);

    public Task<bool> UpdateAsync(MOrganizationJuristicPerson entity)
        => _repository.UpdateAsync(entity);

    public Task<bool> DeleteAsync(string id)
        => _repository.DeleteAsync(id);

    public async Task<IActionResult> BatchEndOfDay_MOrganizationJuristicPerson(searchMOrganizationJuristicPersonModels xmodel)
    {
     
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };
        var JuristicPersonApiResponse = new JuristicPersonApiResponse();
        if (_FlagDev == "Y")
        {
            var filePath = Path.GetFullPath("MocData/Juristic-Person.json", AppContext.BaseDirectory);

            try
            {
                if (File.Exists(filePath))
                {
                    var jsonString = await File.ReadAllTextAsync(filePath);
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<JuristicPersonApiResponse>(jsonString);


                    JuristicPersonApiResponse = result ?? new JuristicPersonApiResponse();
                }
                else
                {
                    JuristicPersonApiResponse = new JuristicPersonApiResponse();
                    Console.WriteLine($"[WARN] File not found: {filePath}");
                }
            }
            catch (Exception ex)
            {
                JuristicPersonApiResponse = new JuristicPersonApiResponse();
                Console.WriteLine($"[ERROR] Failed to read or deserialize {filePath}: {ex.Message}");
            }



        }
        else
        {
            var LApi = await _repositoryApi.GetAllAsync(new MapiInformationModels { ServiceNameCode = "Juristic-Person" });
            var apiParam = LApi.Select(x => new MapiInformationModels
            {
                ServiceNameCode = x.ServiceNameCode,
                ApiKey = x.ApiKey,
                AuthorizationType = x.AuthorizationType,
                ContentType = x.ContentType,
                CreateDate = x.CreateDate,
                Id = x.Id,
                MethodType = x.MethodType,
                ServiceNameTh = x.ServiceNameTh,
                Urldevelopment = x.Urldevelopment,
                Urlproduction = x.Urlproduction,
                Username = x.Username,
                Password = x.Password,
                UpdateDate = x.UpdateDate,
                Bearer = x.Bearer,
                AccessToken = x.AccessToken
                ,AgentId =x.AgentId,
                ConsumerKey =x.ConsumerKey,
                ConsumerSecret =x.ConsumerSecret,
                Token =x.Token,

            }).FirstOrDefault(); // Use FirstOrDefault to handle empty lists

            try
            {
                var apiResponse = await _serviceApi.GetDataApiAsync(apiParam, xmodel);

                // Manually parse the JSON if the structure does not match the model
                var root = Newtonsoft.Json.Linq.JObject.Parse(apiResponse);

                var statusObj = root["status"];
                var status = new Status
                {
                    code = statusObj?["code"]?.ToString(),
                    description = statusObj?["description"]?.ToString()
                };

                var orgJuristicPersonToken = root["data"]?["cd:OrganizationJuristicPerson"];
                var orgJuristicPerson = orgJuristicPersonToken != null
                    ? orgJuristicPersonToken.ToObject<OrganizationJuristicPersonDto>()
                    : null;

                JuristicPersonApiResponse = new JuristicPersonApiResponse
                {
                    status = status,
                    data = orgJuristicPerson != null
                        ? new List<OrganizationJuristicPersonWrapper>
                            {
                    new OrganizationJuristicPersonWrapper
                    {
                        OrganizationJuristicPerson = orgJuristicPerson
                    }
                            }
                        : new List<OrganizationJuristicPersonWrapper>()
                };
            }
            catch (Exception ex)
            {
                // Handle error
            }

        }

        if (JuristicPersonApiResponse.data != null && JuristicPersonApiResponse.data.Any())
        {
            var item = JuristicPersonApiResponse.data.FirstOrDefault()?.OrganizationJuristicPerson;
            if (item != null)
            {
                try
                {
                    var existing = await _repository.GetByIdAsync(item.OrganizationJuristicID);

                    if (existing == null)
                    {
                        // Create new record
                        var newData = new MOrganizationJuristicPerson
                        {
                            OrganizationJuristicId = item.OrganizationJuristicID,
                            OrganizationOldJuristicId = item.OrganizationOldJuristicID,
                            OrganizationJuristicNameTh = item.OrganizationJuristicNameTH,
                            OrganizationJuristicNameEn = item.OrganizationJuristicNameEN,
                            OrganizationJuristicType = item.OrganizationJuristicType,
                            OrganizationJuristicRegisterDate = DateTime.TryParseExact(
                                item.OrganizationJuristicRegisterDate, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out var regDate)
                                ? regDate : null,
                            OrganizationJuristicStatus = item.OrganizationJuristicStatus,
                            OrganizationJuristicObjectiveItems = int.TryParse(item.OrganizationJuristicObjectiveItems, out var itemsCount) ? itemsCount : null,
                            OrganizationJuristicObjectivePages = int.TryParse(item.OrganizationJuristicObjectivePages, out var pages) ? pages : null,
                            OrganizationJuristicRegisterCapital = decimal.TryParse(item.OrganizationJuristicRegisterCapital, out var capital) ? capital : null,
                            OrganizationJuristicPaidUpCapital = decimal.TryParse(item.OrganizationJuristicPaidUpCapital, out var paidUp) ? paidUp : null,
                            OrganizationJuristicBranchName = item.OrganizationJuristicBranchName,
                            DigitalIdflag = item.DigitalIDFlag,

                            // TJuristicObjectives
                            TJuristicObjectives = item.OrganizationJuristicObjective?.Select(obj => new TJuristicObjective
                            {
                                OrganizationJuristicId = item.OrganizationJuristicID,
                                JuristicObjective = obj.JuristicObjective,
                                JuristicObjectiveCode = obj.JuristicObjectiveCode,
                                JuristicObjectiveTextTh = obj.JuristicObjectiveTextTH,
                                JuristicObjectiveTextEn = obj.JuristicObjectiveTextEN
                            }).ToList() ?? new List<TJuristicObjective>(),

                            // TJuristicPersonLists
                            TJuristicPersonLists = item.OrganizationJuristicPersonList?.Select(p => new TJuristicPersonList
                            {
                                OrganizationJuristicId = item.OrganizationJuristicID,
                                JuristicPersonSequence = p.JuristicPersonSequence,
                                JuristicPersonType = p.JuristicPersonType,
                                PersonNameTitleTextTh = p.JuristicPerson?.PersonNameTH?.PersonNameTitleTextTH,
                                PersonFirstNameTh = p.JuristicPerson?.PersonNameTH?.PersonFirstNameTH,
                                PersonMiddleNameTh = p.JuristicPerson?.PersonNameTH?.PersonMiddleNameTH,
                                PersonLastNameTh = p.JuristicPerson?.PersonNameTH?.PersonLastNameTH,
                                JuristicPersonInvestType = p.JuristicPersonInvestType,
                                JuristicPersonInvestAmount = decimal.TryParse(p.JuristicPersonInvestAmount, out var investAmt) ? investAmt : null
                            }).ToList() ?? new List<TJuristicPersonList>(),

                            // TOrganizationJuristicAddresses
                            TOrganizationJuristicAddresses = item.OrganizationJuristicAddress != null
                                ? new List<TOrganizationJuristicAddress>
                                {
                                    new TOrganizationJuristicAddress
                                    {
                                        OrganizationJuristicId = item.OrganizationJuristicID,
                                        Address = item.OrganizationJuristicAddress.AddressType?.Address,
                                        Building = item.OrganizationJuristicAddress.AddressType?.Building,
                                        RoomNo = item.OrganizationJuristicAddress.AddressType?.RoomNo,
                                        Floor = item.OrganizationJuristicAddress.AddressType?.Floor,
                                        AddressNo = item.OrganizationJuristicAddress.AddressType?.AddressNo,
                                        Moo = item.OrganizationJuristicAddress.AddressType?.Moo,
                                        Yaek = item.OrganizationJuristicAddress.AddressType?.Yaek,
                                        Soi = item.OrganizationJuristicAddress.AddressType?.Soi,
                                        Trok = item.OrganizationJuristicAddress.AddressType?.Trok,
                                        Village = item.OrganizationJuristicAddress.AddressType?.Village,
                                        Road = item.OrganizationJuristicAddress.AddressType?.Road,
                                        CitySubDivisionCode = item.OrganizationJuristicAddress.AddressType?.CitySubDivision?.CitySubDivisionCode,
                                        CitySubDivisionTextTh = item.OrganizationJuristicAddress.AddressType?.CitySubDivision?.CitySubDivisionTextTH,
                                        CityCode = item.OrganizationJuristicAddress.AddressType?.City?.CityCode,
                                        CityTextTh = item.OrganizationJuristicAddress.AddressType?.City?.CityTextTH,
                                        CountrySubDivisionCode = item.OrganizationJuristicAddress.AddressType?.CountrySubDivision?.CountrySubDivisionCode,
                                        CountrySubDivisionTextTh = item.OrganizationJuristicAddress.AddressType?.CountrySubDivision?.CountrySubDivisionTextTH
                                    }
                                }
                                : new List<TOrganizationJuristicAddress>(),

                            // TOrganizationJuristicPersonDescriptions
                            TOrganizationJuristicPersonDescriptions = item.OrganizationJuristicPersonDescription?.Select(d => new TOrganizationJuristicPersonDescription
                            {
                                OrganizationJuristicId = item.OrganizationJuristicID,
                                DescriptionSequence = d.OrganizationJuristicPersonDescriptionSequence,
                                DescriptionType = d.OrganizationJuristicPersonDescriptionType,
                                DescriptionDetail = d.OrganizationJuristicPersonDescriptionDetail
                            }).ToList() ?? new List<TOrganizationJuristicPersonDescription>()
                        };

                        await _repository.AddAsync(newData);
                        Console.WriteLine($"[INFO] Created new MOrganizationJuristicPerson with ID {newData.OrganizationJuristicId}");
                    }
                    else
                    {
                        // Update existing record
                        existing.OrganizationOldJuristicId = item.OrganizationOldJuristicID;
                        existing.OrganizationJuristicNameTh = item.OrganizationJuristicNameTH;
                        existing.OrganizationJuristicNameEn = item.OrganizationJuristicNameEN;
                        existing.OrganizationJuristicType = item.OrganizationJuristicType;
                        existing.OrganizationJuristicRegisterDate = DateTime.TryParseExact(
                            item.OrganizationJuristicRegisterDate, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out var regDate)
                            ? regDate : null;
                        existing.OrganizationJuristicStatus = item.OrganizationJuristicStatus;
                        existing.OrganizationJuristicObjectiveItems = int.TryParse(item.OrganizationJuristicObjectiveItems, out var items) ? items : null;
                        existing.OrganizationJuristicObjectivePages = int.TryParse(item.OrganizationJuristicObjectivePages, out var pages) ? pages : null;
                        existing.OrganizationJuristicRegisterCapital = decimal.TryParse(item.OrganizationJuristicRegisterCapital, out var capital) ? capital : null;
                        existing.OrganizationJuristicPaidUpCapital = decimal.TryParse(item.OrganizationJuristicPaidUpCapital, out var paidUp) ? paidUp : null;
                        existing.OrganizationJuristicBranchName = item.OrganizationJuristicBranchName;
                        existing.DigitalIdflag = item.DigitalIDFlag;

                        // --- Update child collections ---
                        // (Child collection updates remain unchanged)
                        await _repository.UpdateAsync(existing);
                        Console.WriteLine($"[INFO] Updated MOrganizationJuristicPerson with ID {existing.OrganizationJuristicId}");
                    }
                }
                catch (Exception ex)
                {
                    var errorResponse = new ErrorResponseModels
                    {
                        responseCode = "500",
                        responseMsg = ex.Message
                    };

                    return new JsonResult(errorResponse)
                    {
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                }
            }
        }

       

        var successResponse = new ErrorResponseModels
        {
            responseCode = "200",
            responseMsg = "Batch processing completed successfully."
        };
        return new JsonResult(successResponse)
        {
            StatusCode = StatusCodes.Status200OK
        };
    }

    public async Task<JuristicPersonApiResponse> GetAllAsyncSearch_JuristicPerson(searchMOrganizationJuristicPersonModels xmodel)
    {
        try
        {
            if (string.IsNullOrEmpty(xmodel.OrganizationJuristicID))
            {
                var entities = await _repository.GetAllAsync();
                var wrapperList = entities.Select(entity => new OrganizationJuristicPersonWrapper
                {
                    OrganizationJuristicPerson = new OrganizationJuristicPersonDto
                    {
                        OrganizationJuristicID = entity.OrganizationJuristicId,
                        OrganizationOldJuristicID = entity.OrganizationOldJuristicId,
                        OrganizationJuristicNameTH = entity.OrganizationJuristicNameTh,
                        OrganizationJuristicNameEN = entity.OrganizationJuristicNameEn,
                        OrganizationJuristicType = entity.OrganizationJuristicType,
                        OrganizationJuristicRegisterDate = entity.OrganizationJuristicRegisterDate?.ToString("yyyyMMdd"),
                        OrganizationJuristicStatus = entity.OrganizationJuristicStatus,
                        OrganizationJuristicObjective = entity.TJuristicObjectives?.Select(obj => new JuristicObjectiveDto
                        {
                            JuristicObjective = obj.JuristicObjective,
                            JuristicObjectiveCode = obj.JuristicObjectiveCode,
                            JuristicObjectiveTextTH = obj.JuristicObjectiveTextTh,
                            JuristicObjectiveTextEN = obj.JuristicObjectiveTextEn
                        }).ToList(),
                        OrganizationJuristicObjectiveItems = entity.OrganizationJuristicObjectiveItems?.ToString(),
                        OrganizationJuristicObjectivePages = entity.OrganizationJuristicObjectivePages?.ToString(),
                        OrganizationJuristicRegisterCapital = entity.OrganizationJuristicRegisterCapital?.ToString(),
                        OrganizationJuristicPaidUpCapital = entity.OrganizationJuristicPaidUpCapital?.ToString(),
                        OrganizationJuristicPersonList = entity.TJuristicPersonLists?.Select(p => new JuristicPersonListDto
                        {
                            JuristicPersonSequence = p.JuristicPersonSequence ?? 0,
                            JuristicPersonType = p.JuristicPersonType,
                            JuristicPerson = new JuristicPersonDto
                            {
                                PersonNameTH = new PersonNameThDto
                                {
                                    PersonNameTitleTextTH = p.PersonNameTitleTextTh,
                                    PersonFirstNameTH = p.PersonFirstNameTh,
                                    PersonMiddleNameTH = p.PersonMiddleNameTh,
                                    PersonLastNameTH = p.PersonLastNameTh
                                }
                            },
                            JuristicPersonInvestType = p.JuristicPersonInvestType,
                            JuristicPersonInvestAmount = p.JuristicPersonInvestAmount?.ToString()
                        }).ToList(),
                        OrganizationJuristicBranchName = entity.OrganizationJuristicBranchName,
                        OrganizationJuristicAddress = entity.TOrganizationJuristicAddresses?.FirstOrDefault() != null
                            ? new OrganizationJuristicAddressDto
                            {
                                AddressType = new AddressTypeDto
                                {
                                    Address = entity.TOrganizationJuristicAddresses.FirstOrDefault()?.Address,
                                    Building = entity.TOrganizationJuristicAddresses.FirstOrDefault()?.Building,
                                    RoomNo = entity.TOrganizationJuristicAddresses.FirstOrDefault()?.RoomNo,
                                    Floor = entity.TOrganizationJuristicAddresses.FirstOrDefault()?.Floor,
                                    AddressNo = entity.TOrganizationJuristicAddresses.FirstOrDefault()?.AddressNo,
                                    Moo = entity.TOrganizationJuristicAddresses.FirstOrDefault()?.Moo,
                                    Yaek = entity.TOrganizationJuristicAddresses.FirstOrDefault()?.Yaek,
                                    Soi = entity.TOrganizationJuristicAddresses.FirstOrDefault()?.Soi,
                                    Trok = entity.TOrganizationJuristicAddresses.FirstOrDefault()?.Trok,
                                    Village = entity.TOrganizationJuristicAddresses.FirstOrDefault()?.Village,
                                    Road = entity.TOrganizationJuristicAddresses.FirstOrDefault()?.Road,
                                    CitySubDivision = new CitySubDivisionDto
                                    {
                                        CitySubDivisionCode = entity.TOrganizationJuristicAddresses.FirstOrDefault()?.CitySubDivisionCode,
                                        CitySubDivisionTextTH = entity.TOrganizationJuristicAddresses.FirstOrDefault()?.CitySubDivisionTextTh
                                    },
                                    City = new CityDto
                                    {
                                        CityCode = entity.TOrganizationJuristicAddresses.FirstOrDefault()?.CityCode,
                                        CityTextTH = entity.TOrganizationJuristicAddresses.FirstOrDefault()?.CityTextTh
                                    },
                                    CountrySubDivision = new CountrySubDivisionDto
                                    {
                                        CountrySubDivisionCode = entity.TOrganizationJuristicAddresses.FirstOrDefault()?.CountrySubDivisionCode,
                                        CountrySubDivisionTextTH = entity.TOrganizationJuristicAddresses.FirstOrDefault()?.CountrySubDivisionTextTh
                                    }
                                }
                            }
                            : null,
                        OrganizationJuristicPersonDescription = entity.TOrganizationJuristicPersonDescriptions?.Select(d => new OrganizationJuristicPersonDescriptionDto
                        {
                            OrganizationJuristicPersonDescriptionSequence = d.DescriptionSequence,
                            OrganizationJuristicPersonDescriptionType = d.DescriptionType,
                            OrganizationJuristicPersonDescriptionDetail = d.DescriptionDetail
                        }).ToList(),
                        DigitalIDFlag = entity.DigitalIdflag
                    }
                }).ToList();

                return new JuristicPersonApiResponse
                {
                    status = new Status
                    {
                        code = "200",
                        description = "Success"
                    },
                    data = wrapperList // <<-- data เป็น List<OrganizationJuristicPersonWrapper>
                };
            }
            else {
                var entity = await _repository.GetByIdAsync(xmodel.OrganizationJuristicID);

                if (entity == null)
                {
                    var xdata = await BatchEndOfDay_MOrganizationJuristicPerson(xmodel);


                    entity = await _repository.GetByIdAsync(xmodel.OrganizationJuristicID);
                    if (entity == null)
                    {
                        return new JuristicPersonApiResponse
                        {
                            status = new Status
                            {
                                code = "200",
                                description = "Not found"
                            },
                            data = new List<OrganizationJuristicPersonWrapper>() // Return an empty list instead of JuristicPersonData
                        };
                    }

                }

                var response = new JuristicPersonApiResponse
                {
                    status = new Status
                    {
                        code = "200",
                        description = "Success"
                    },
                    data = new List<OrganizationJuristicPersonWrapper>
                    {
                        new OrganizationJuristicPersonWrapper
                        {
                            OrganizationJuristicPerson = new OrganizationJuristicPersonDto
                            {
                                OrganizationJuristicID = entity.OrganizationJuristicId,
                                OrganizationOldJuristicID = entity.OrganizationOldJuristicId,
                                OrganizationJuristicNameTH = entity.OrganizationJuristicNameTh,
                                OrganizationJuristicNameEN = entity.OrganizationJuristicNameEn,
                                OrganizationJuristicType = entity.OrganizationJuristicType,
                                OrganizationJuristicRegisterDate = entity.OrganizationJuristicRegisterDate?.ToString("yyyyMMdd"),
                                OrganizationJuristicStatus = entity.OrganizationJuristicStatus,
                                OrganizationJuristicObjective = entity.TJuristicObjectives?.Select(obj => new JuristicObjectiveDto
                                {
                                    JuristicObjective = obj.JuristicObjective,
                                    JuristicObjectiveCode = obj.JuristicObjectiveCode,
                                    JuristicObjectiveTextTH = obj.JuristicObjectiveTextTh,
                                    JuristicObjectiveTextEN = obj.JuristicObjectiveTextEn
                                }).ToList(),
                                OrganizationJuristicObjectiveItems = entity.OrganizationJuristicObjectiveItems?.ToString(),
                                OrganizationJuristicObjectivePages = entity.OrganizationJuristicObjectivePages?.ToString(),
                                OrganizationJuristicRegisterCapital = entity.OrganizationJuristicRegisterCapital?.ToString(),
                                OrganizationJuristicPaidUpCapital = entity.OrganizationJuristicPaidUpCapital?.ToString(),
                                OrganizationJuristicPersonList = entity.TJuristicPersonLists?.Select(p => new JuristicPersonListDto
                                {
                                    JuristicPersonSequence = p.JuristicPersonSequence ?? 0,
                                    JuristicPersonType = p.JuristicPersonType,
                                    JuristicPerson = new JuristicPersonDto
                                    {
                                        PersonNameTH = new PersonNameThDto
                                        {
                                            PersonNameTitleTextTH = p.PersonNameTitleTextTh,
                                            PersonFirstNameTH = p.PersonFirstNameTh,
                                            PersonMiddleNameTH = p.PersonMiddleNameTh,
                                            PersonLastNameTH = p.PersonLastNameTh
                                        }
                                    },
                                    JuristicPersonInvestType = p.JuristicPersonInvestType,
                                    JuristicPersonInvestAmount = p.JuristicPersonInvestAmount?.ToString()
                                }).ToList(),
                                OrganizationJuristicBranchName = entity.OrganizationJuristicBranchName,
                                OrganizationJuristicAddress = entity.TOrganizationJuristicAddresses?.FirstOrDefault() != null
                                    ? new OrganizationJuristicAddressDto
                                    {
                                        AddressType = new AddressTypeDto
                                        {
                                            Address = entity.TOrganizationJuristicAddresses.FirstOrDefault()?.Address,
                                            Building = entity.TOrganizationJuristicAddresses.FirstOrDefault()?.Building,
                                            RoomNo = entity.TOrganizationJuristicAddresses.FirstOrDefault()?.RoomNo,
                                            Floor = entity.TOrganizationJuristicAddresses.FirstOrDefault()?.Floor,
                                            AddressNo = entity.TOrganizationJuristicAddresses.FirstOrDefault()?.AddressNo,
                                            Moo = entity.TOrganizationJuristicAddresses.FirstOrDefault()?.Moo,
                                            Yaek = entity.TOrganizationJuristicAddresses.FirstOrDefault()?.Yaek,
                                            Soi = entity.TOrganizationJuristicAddresses.FirstOrDefault()?.Soi,
                                            Trok = entity.TOrganizationJuristicAddresses.FirstOrDefault()?.Trok,
                                            Village = entity.TOrganizationJuristicAddresses.FirstOrDefault()?.Village,
                                            Road = entity.TOrganizationJuristicAddresses.FirstOrDefault()?.Road,
                                            CitySubDivision = new CitySubDivisionDto
                                            {
                                                CitySubDivisionCode = entity.TOrganizationJuristicAddresses.FirstOrDefault()?.CitySubDivisionCode,
                                                CitySubDivisionTextTH = entity.TOrganizationJuristicAddresses.FirstOrDefault()?.CitySubDivisionTextTh
                                            },
                                            City = new CityDto
                                            {
                                                CityCode = entity.TOrganizationJuristicAddresses.FirstOrDefault()?.CityCode,
                                                CityTextTH = entity.TOrganizationJuristicAddresses.FirstOrDefault()?.CityTextTh
                                            },
                                            CountrySubDivision = new CountrySubDivisionDto
                                            {
                                                CountrySubDivisionCode = entity.TOrganizationJuristicAddresses.FirstOrDefault()?.CountrySubDivisionCode,
                                                CountrySubDivisionTextTH = entity.TOrganizationJuristicAddresses.FirstOrDefault()?.CountrySubDivisionTextTh
                                            }
                                        }
                                    }
                                    : null,
                                OrganizationJuristicPersonDescription = entity.TOrganizationJuristicPersonDescriptions?.Select(d => new OrganizationJuristicPersonDescriptionDto
                                {
                                    OrganizationJuristicPersonDescriptionSequence = d.DescriptionSequence,
                                    OrganizationJuristicPersonDescriptionType = d.DescriptionType,
                                    OrganizationJuristicPersonDescriptionDetail = d.DescriptionDetail
                                }).ToList(),
                                DigitalIDFlag = entity.DigitalIdflag
                            }
                        }
                    }
                };

                return response;
            }

       
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Exception in GetAllAsyncSearch_JuristicPerson: {ex.Message}");
            return new JuristicPersonApiResponse
            {
                status = new Status
                {
                    code = "500",
                    description = "Internal Server Error"
                },
                data = new List<OrganizationJuristicPersonWrapper>() // Return an empty list instead of JuristicPersonData
            };
        }

    }

}
