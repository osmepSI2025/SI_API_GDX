using System;
using System.Collections.Generic;

namespace SME_API_GDX.Entities;

public partial class TJuristicObjective
{
    public int ObjectiveId { get; set; }

    public string? OrganizationJuristicId { get; set; }

    public string? JuristicObjective { get; set; }

    public string? JuristicObjectiveCode { get; set; }

    public string? JuristicObjectiveTextTh { get; set; }

    public string? JuristicObjectiveTextEn { get; set; }

    public virtual MOrganizationJuristicPerson? OrganizationJuristic { get; set; }
}
