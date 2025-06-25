using Microsoft.EntityFrameworkCore;

namespace JobHunt.Core.Domain.ValueObjects;

[PrimaryKey(nameof(JobFieldId), nameof(LanguageCode))]
public class JobFieldLang
{
    public JobField? JobFieldId { get; set; }
    public string? LanguageCode { get; set; }
    public string? Name { get; set; }
}