public class Status
{
    public string code { get; set; }
    public string description { get; set; }
}

public class JuristicPersonApiResponse
{
    public Status status { get; set; }
    public List<OrganizationJuristicPersonWrapper> data { get; set; }
}
// Add this class definition to resolve the missing type
public class OrganizationJuristicPersonWrapper
{
    public OrganizationJuristicPersonDto OrganizationJuristicPerson { get; set; }
}
public class JuristicPersonData
{
    // สำหรับกรณีคืนค่ารายเดียว (เดิม)
    [Newtonsoft.Json.JsonProperty("cd:OrganizationJuristicPerson")]
    public OrganizationJuristicPersonDto OrganizationJuristicPerson { get; set; }

    // สำหรับกรณีคืนค่าหลายรายการ (ใหม่)
    [Newtonsoft.Json.JsonProperty("cd:OrganizationJuristicPersonList")]
    public List<OrganizationJuristicPersonDto> OrganizationJuristicPersonList { get; set; }
}

public class OrganizationJuristicPersonDto
{
    [Newtonsoft.Json.JsonProperty("cd:OrganizationJuristicID")]
    public string OrganizationJuristicID { get; set; }

    [Newtonsoft.Json.JsonProperty("cd:OrganizationOldJuristicID")]
    public string OrganizationOldJuristicID { get; set; }

    [Newtonsoft.Json.JsonProperty("cd:OrganizationJuristicNameTH")]
    public string OrganizationJuristicNameTH { get; set; }

    [Newtonsoft.Json.JsonProperty("cd:OrganizationJuristicNameEN")]
    public string OrganizationJuristicNameEN { get; set; }

    [Newtonsoft.Json.JsonProperty("cd:OrganizationJuristicType")]
    public string OrganizationJuristicType { get; set; }

    [Newtonsoft.Json.JsonProperty("cd:OrganizationJuristicRegisterDate")]
    public string OrganizationJuristicRegisterDate { get; set; }

    [Newtonsoft.Json.JsonProperty("cd:OrganizationJuristicStatus")]
    public string OrganizationJuristicStatus { get; set; }

    [Newtonsoft.Json.JsonProperty("cd:OrganizationJuristicObjective")]
    public List<JuristicObjectiveDto> OrganizationJuristicObjective { get; set; }

    [Newtonsoft.Json.JsonProperty("cd:OrganizationJuristicObjectiveItems")]
    public string OrganizationJuristicObjectiveItems { get; set; }

    [Newtonsoft.Json.JsonProperty("cd:OrganizationJuristicObjectivePages")]
    public string OrganizationJuristicObjectivePages { get; set; }

    [Newtonsoft.Json.JsonProperty("cd:OrganizationJuristicRegisterCapital")]
    public string OrganizationJuristicRegisterCapital { get; set; }

    [Newtonsoft.Json.JsonProperty("cd:OrganizationJuristicPaidUpCapital")]
    public string OrganizationJuristicPaidUpCapital { get; set; }

    [Newtonsoft.Json.JsonProperty("cd:OrganizationJuristicPersonList")]
    public List<JuristicPersonListDto> OrganizationJuristicPersonList { get; set; }

    [Newtonsoft.Json.JsonProperty("cd:OrganizationJuristicBranchName")]
    public string OrganizationJuristicBranchName { get; set; }

    [Newtonsoft.Json.JsonProperty("cd:OrganizationJuristicAddress")]
    public OrganizationJuristicAddressDto OrganizationJuristicAddress { get; set; }

    [Newtonsoft.Json.JsonProperty("cd:OrganizationJuristicPersonDescription")]
    public List<OrganizationJuristicPersonDescriptionDto> OrganizationJuristicPersonDescription { get; set; }

    [Newtonsoft.Json.JsonProperty("td:DigitalIDFlag")]
    public string DigitalIDFlag { get; set; }
}

public class JuristicObjectiveDto
{
    [Newtonsoft.Json.JsonProperty("td:JuristicObjective")]
    public string JuristicObjective { get; set; }

    [Newtonsoft.Json.JsonProperty("td:JuristicObjectiveCode")]
    public string JuristicObjectiveCode { get; set; }

    [Newtonsoft.Json.JsonProperty("td:JuristicObjectiveTextTH")]
    public string JuristicObjectiveTextTH { get; set; }

    [Newtonsoft.Json.JsonProperty("td:JuristicObjectiveTextEN")]
    public string JuristicObjectiveTextEN { get; set; }
}

public class JuristicPersonListDto
{
    [Newtonsoft.Json.JsonProperty("td:JuristicPersonSequence")]
    public int JuristicPersonSequence { get; set; }

    [Newtonsoft.Json.JsonProperty("td:JuristicPersonType")]
    public string JuristicPersonType { get; set; }

    [Newtonsoft.Json.JsonProperty("td:JuristicPerson")]
    public JuristicPersonDto JuristicPerson { get; set; }

    [Newtonsoft.Json.JsonProperty("td:JuristicPersonInvestType")]
    public string JuristicPersonInvestType { get; set; }

    [Newtonsoft.Json.JsonProperty("td:JuristicPersonInvestAmount")]
    public string JuristicPersonInvestAmount { get; set; }
}

public class JuristicPersonDto
{
    [Newtonsoft.Json.JsonProperty("cd:PersonNameTH")]
    public PersonNameThDto PersonNameTH { get; set; }
}

public class PersonNameThDto
{
    [Newtonsoft.Json.JsonProperty("cd:PersonNameTitleTextTH")]
    public string PersonNameTitleTextTH { get; set; }

    [Newtonsoft.Json.JsonProperty("cd:PersonFirstNameTH")]
    public string PersonFirstNameTH { get; set; }

    [Newtonsoft.Json.JsonProperty("cd:PersonMiddleNameTH")]
    public string PersonMiddleNameTH { get; set; }

    [Newtonsoft.Json.JsonProperty("cd:PersonLastNameTH")]
    public string PersonLastNameTH { get; set; }
}

public class OrganizationJuristicAddressDto
{
    [Newtonsoft.Json.JsonProperty("cr:AddressType")]
    public AddressTypeDto AddressType { get; set; }
}

public class AddressTypeDto
{
    [Newtonsoft.Json.JsonProperty("cd:Address")]
    public string Address { get; set; }

    [Newtonsoft.Json.JsonProperty("cd:Building")]
    public string Building { get; set; }

    [Newtonsoft.Json.JsonProperty("cd:RoomNo")]
    public string RoomNo { get; set; }

    [Newtonsoft.Json.JsonProperty("cd:Floor")]
    public string Floor { get; set; }

    [Newtonsoft.Json.JsonProperty("cd:AddressNo")]
    public string AddressNo { get; set; }

    [Newtonsoft.Json.JsonProperty("cd:Moo")]
    public string Moo { get; set; }

    [Newtonsoft.Json.JsonProperty("cd:Yaek")]
    public string Yaek { get; set; }

    [Newtonsoft.Json.JsonProperty("cd:Soi")]
    public string Soi { get; set; }

    [Newtonsoft.Json.JsonProperty("cd:Trok")]
    public string Trok { get; set; }

    [Newtonsoft.Json.JsonProperty("cd:Village")]
    public string Village { get; set; }

    [Newtonsoft.Json.JsonProperty("cd:Road")]
    public string Road { get; set; }

    [Newtonsoft.Json.JsonProperty("cd:CitySubDivision")]
    public CitySubDivisionDto CitySubDivision { get; set; }

    [Newtonsoft.Json.JsonProperty("cd:City")]
    public CityDto City { get; set; }

    [Newtonsoft.Json.JsonProperty("cd:CountrySubDivision")]
    public CountrySubDivisionDto CountrySubDivision { get; set; }
}

public class CitySubDivisionDto
{
    [Newtonsoft.Json.JsonProperty("cr:CitySubDivisionCode")]
    public string CitySubDivisionCode { get; set; }

    [Newtonsoft.Json.JsonProperty("cr:CitySubDivisionTextTH")]
    public string CitySubDivisionTextTH { get; set; }
}

public class CityDto
{
    [Newtonsoft.Json.JsonProperty("cr:CityCode")]
    public string CityCode { get; set; }

    [Newtonsoft.Json.JsonProperty("cr:CityTextTH")]
    public string CityTextTH { get; set; }
}

public class CountrySubDivisionDto
{
    [Newtonsoft.Json.JsonProperty("cr:CountrySubDivisionCode")]
    public string CountrySubDivisionCode { get; set; }

    [Newtonsoft.Json.JsonProperty("cr:CountrySubDivisionTextTH")]
    public string CountrySubDivisionTextTH { get; set; }
}

public class OrganizationJuristicPersonDescriptionDto
{
    [Newtonsoft.Json.JsonProperty("cd:OrganizationJuristicPersonDescriptionSequence")]
    public int? OrganizationJuristicPersonDescriptionSequence { get; set; }

    [Newtonsoft.Json.JsonProperty("cd:OrganizationJuristicPersonDescriptionType")]
    public string OrganizationJuristicPersonDescriptionType { get; set; }

    [Newtonsoft.Json.JsonProperty("cd:OrganizationJuristicPersonDescriptionDetail")]
    public string OrganizationJuristicPersonDescriptionDetail { get; set; }
}

public class searchMOrganizationJuristicPersonModels
{
  
    public string OrganizationJuristicID { get; set; }
}
