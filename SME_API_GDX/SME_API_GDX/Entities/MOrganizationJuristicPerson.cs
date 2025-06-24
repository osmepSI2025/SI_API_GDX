using System;
using System.Collections.Generic;

namespace SME_API_GDX.Entities;

public partial class MOrganizationJuristicPerson
{
    public string OrganizationJuristicId { get; set; } = null!;

    public string? OrganizationOldJuristicId { get; set; }

    public string? OrganizationJuristicNameTh { get; set; }

    public string? OrganizationJuristicNameEn { get; set; }

    public string? OrganizationJuristicType { get; set; }

    public DateTime? OrganizationJuristicRegisterDate { get; set; }

    public string? OrganizationJuristicStatus { get; set; }

    public int? OrganizationJuristicObjectiveItems { get; set; }

    public int? OrganizationJuristicObjectivePages { get; set; }

    public decimal? OrganizationJuristicRegisterCapital { get; set; }

    public decimal? OrganizationJuristicPaidUpCapital { get; set; }

    public string? OrganizationJuristicBranchName { get; set; }

    public string? DigitalIdflag { get; set; }

    public virtual ICollection<TJuristicObjective> TJuristicObjectives { get; set; } = new List<TJuristicObjective>();

    public virtual ICollection<TJuristicPersonList> TJuristicPersonLists { get; set; } = new List<TJuristicPersonList>();

    public virtual ICollection<TOrganizationJuristicAddress> TOrganizationJuristicAddresses { get; set; } = new List<TOrganizationJuristicAddress>();

    public virtual ICollection<TOrganizationJuristicPersonDescription> TOrganizationJuristicPersonDescriptions { get; set; } = new List<TOrganizationJuristicPersonDescription>();
}
