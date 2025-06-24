using System;
using System.Collections.Generic;

namespace SME_API_GDX.Entities;

public partial class TJuristicPersonList
{
    public int PersonListId { get; set; }

    public string? OrganizationJuristicId { get; set; }

    public int? JuristicPersonSequence { get; set; }

    public string? JuristicPersonType { get; set; }

    public string? PersonNameTitleTextTh { get; set; }

    public string? PersonFirstNameTh { get; set; }

    public string? PersonMiddleNameTh { get; set; }

    public string? PersonLastNameTh { get; set; }

    public string? JuristicPersonInvestType { get; set; }

    public decimal? JuristicPersonInvestAmount { get; set; }

    public virtual MOrganizationJuristicPerson? OrganizationJuristic { get; set; }
}
