using System.ComponentModel.DataAnnotations;
using JobHunt.Core.Domain.Entities;

namespace JobHunt.Core.Domain.ValueObjects;

public enum MajorKey
{
    // Default
    None,

    // Computer Science & Engineering
    Computer_Science,
    Information_Technology,
    Software_Engineering,
    Data_Science,
    Artificial_Intelligence,
    Cybersecurity,
    Computer_Engineering,

    // Engineering
    Electrical_Engineering,
    Electronics_Engineering,
    Mechanical_Engineering,
    Mechatronics_Engineering,
    Civil_Engineering,
    Transportation_Engineering,
    Environmental_Engineering,
    Industrial_Engineering,
    Automation_Engineering,
    Biomedical_Engineering,

    // Business & Management
    Business_Administration,
    Marketing,
    Finance,
    Banking,
    Accounting,
    International_Business,
    Human_Resources,
    Logistics_And_Supply_Chain,
    Hospitality_Management,
    Tourism_Management,
    E_Commerce,
    Real_Estate,

    // Arts, Design & Media
    Arts_And_Design,
    Graphic_Design,
    Interior_Design,
    Industrial_Design,
    Multimedia_Design,
    Fashion_Design,
    Architecture,
    Media_And_Communication,
    Journalism,
    Public_Relations,
    Film_And_Television,

    // Natural Sciences
    Mathematics,
    Physics,
    Chemistry,
    Biology,
    Environmental_Science,
    Food_Science_And_Technology,
    Geology,
    Marine_Science,

    // Life Sciences & Health
    Medicine,
    Pharmacy,
    Dentistry,
    Nursing,
    Medical_Laboratory_Technology,
    Nutrition,
    Public_Health,
    Psychology,

    // Social Sciences & Humanities
    Sociology,
    Anthropology,
    Philosophy,
    History,
    Political_Science,
    International_Relations,
    Law,
    Public_Administration,
    Social_Work,
    Education,
    Linguistics,
    Library_And_Information_Science,

    // Agriculture & Environment
    Agronomy,
    Animal_Science,
    Veterinary_Medicine,
    Aquaculture,
    Agricultural_Economics,
    Forestry,
    Climate_Change_Studies
}

public class Major
{
    [Key]
    public MajorKey MajorId { get; set; }

    [Required]
    [MaxLength(64)]
    public string? VietNameseName { get; set; }
    
    public List<JobHunter> JobHunters { get; set; } = [];
}