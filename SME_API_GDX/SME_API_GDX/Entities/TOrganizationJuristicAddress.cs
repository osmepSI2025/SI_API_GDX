using System;
using System.Collections.Generic;

namespace SME_API_GDX.Entities;

public partial class TOrganizationJuristicAddress
{
    public int AddressId { get; set; }

    public string? OrganizationJuristicId { get; set; }

    public string? Address { get; set; }

    public string? Building { get; set; }

    public string? RoomNo { get; set; }

    public string? Floor { get; set; }

    public string? AddressNo { get; set; }

    public string? Moo { get; set; }

    public string? Yaek { get; set; }

    public string? Soi { get; set; }

    public string? Trok { get; set; }

    public string? Village { get; set; }

    public string? Road { get; set; }

    public string? CitySubDivisionCode { get; set; }

    public string? CitySubDivisionTextTh { get; set; }

    public string? CityCode { get; set; }

    public string? CityTextTh { get; set; }

    public string? CountrySubDivisionCode { get; set; }

    public string? CountrySubDivisionTextTh { get; set; }

    public virtual MOrganizationJuristicPerson? OrganizationJuristic { get; set; }
}
