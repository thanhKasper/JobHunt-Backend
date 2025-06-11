using JobHunt.Core.DTO;
using JobHunt.Core.ServiceContracts;
using JobHunt.Core.Services;

namespace JobHunt.ServiceTests;

public class JobFilterServiceTest
{
    private readonly IJobFilterService _jobfilterService = new JobFilterService();

    [Fact]
    public async Task CreateNewJobFilter_EmptyArgument()
    {
        // Preparation
        JobFilterCreationRequest? filterReq = null;

        // Action
        async Task testAction()
        {
            await _jobfilterService.CreateNewJobFilter(filterReq);
        }

        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(testAction);
    }

    [Fact]
    public async Task CreateNewJobFilter_EmptyJobFilterName()
    {
        JobFilterCreationRequest? req = new()
        {
            FilterTitle = null,
            Languages = ["Vietnamese", "English"],
            Level = "intern",
            Occupation = "IT",
            SoftSkills = ["Teamwork", "Presentation", "Communication"],
            TechnicalKnowledge = ["TCP/IP", "Web", "Frontend", "OOP"],
            Tools = ["HTML", "CSS", "Javascript", "Typescript"]
        };

        await Assert.ThrowsAsync<ArgumentException>(
            async () => await _jobfilterService.CreateNewJobFilter(req)
        );
    }

    [Fact]
    public async Task CreateNewJobFitler_EmptyJobLevelAndYearsOfExperience()
    {
        JobFilterCreationRequest? req = new()
        {
            FilterTitle = "Lap Trinh Vien Frontend",
            Languages = ["Vietnamese", "English"],
            Level = null,
            YearsOfExperience = null,
            Occupation = "IT",
            SoftSkills = ["Teamwork", "Presentation", "Communication"],
            TechnicalKnowledge = ["TCP/IP", "Web", "Frontend", "OOP"],
            Tools = ["HTML", "CSS", "Javascript", "Typescript"]
        };

        await Assert.ThrowsAsync<ArgumentException>(
            async () => await _jobfilterService.CreateNewJobFilter(req)
        );
    }

    [Fact]
    public async Task CreateNewJobFilter_EmptyOccupation()
    {
        JobFilterCreationRequest? req = new()
        {
            FilterTitle = "Lap Trinh Vien Frontend",
            Languages = ["Vietnamese", "English"],
            Level = "fresher",
            YearsOfExperience = 1,
            Occupation = null,
            SoftSkills = ["Teamwork", "Presentation", "Communication"],
            TechnicalKnowledge = ["TCP/IP", "Web", "Frontend", "OOP"],
            Tools = ["HTML", "CSS", "Javascript", "Typescript"]
        };

        await Assert.ThrowsAsync<ArgumentException>(
            async () => await _jobfilterService.CreateNewJobFilter(req)
        );
    }

    [Fact]
    public async Task CreateNewJobFilter_ProperJobFilterCreation()
    {
        JobFilterCreationRequest? req = new()
        {
            FilterTitle = "Lap Trinh Vien Frontend",
            Languages = ["Vietnamese", "English"],
            Level = "fresher",
            YearsOfExperience = 1,
            Occupation = "Information Technology",
            SoftSkills = ["Teamwork", "Presentation", "Communication"],
            TechnicalKnowledge = ["TCP/IP", "Web", "Frontend", "OOP"],
            Tools = ["HTML", "CSS", "Javascript", "Typescript"]
        };

        JobFilterResponseSimple res = await _jobfilterService.CreateNewJobFilter(req);

        Assert.True(res.Id != Guid.Empty);
    }

    [Fact]
    public async Task CreateNewJobFilter_AutoCreateJobLevelWhenYearsOfExperienceIsProvided()
    {
        JobFilterCreationRequest? req = new()
        {
            FilterTitle = "Lap Trinh Vien Frontend",
            Languages = ["Vietnamese", "English"],
            Level = null,
            YearsOfExperience = 1,
            Occupation = "Information Technology",
            SoftSkills = ["Teamwork", "Presentation", "Communication"],
            TechnicalKnowledge = ["TCP/IP", "Web", "Frontend", "OOP"],
            Tools = ["HTML", "CSS", "Javascript", "Typescript"]
        };

        JobFilterResponseSimple res = await _jobfilterService.CreateNewJobFilter(req);
        JobFilterResponseDetail jobFilterDetail = await _jobfilterService.GetJobFilterDetail(res.Id);

        Assert.NotNull(jobFilterDetail.Level);
    }

    [Fact]
    public async Task CreateNewJobFilter_AutoCreateYearsOfExperienceWhenJobLevelIsProvided()
    {
        JobFilterCreationRequest? req = new()
        {
            FilterTitle = "Lap Trinh Vien Frontend",
            Languages = ["Vietnamese", "English"],
            Level = "fresher",
            YearsOfExperience = null,
            Occupation = "Information Technology",
            SoftSkills = ["Teamwork", "Presentation", "Communication"],
            TechnicalKnowledge = ["TCP/IP", "Web", "Frontend", "OOP"],
            Tools = ["HTML", "CSS", "Javascript", "Typescript"]
        };

        JobFilterResponseSimple res = await _jobfilterService.CreateNewJobFilter(req);
        JobFilterResponseDetail jobFilterDetail = await _jobfilterService.GetJobFilterDetail(res.Id);

        Assert.NotNull(jobFilterDetail.Level);
    }

    [Fact]
    public async Task CreateNewJobFilter_RemoveDuplicateKeywordsInSoftSkillsField()
    {
        JobFilterCreationRequest? req = new()
        {
            FilterTitle = "Lap Trinh Vien Frontend",
            Languages = ["Vietnamese", "English"],
            Level = "fresher",
            YearsOfExperience = null,
            Occupation = "Information Technology",
            SoftSkills = ["Teamwork", "Presentation", "Communication", "communication"],
            TechnicalKnowledge = ["TCP/IP", "Web", "Frontend", "OOP"],
            Tools = ["HTML", "CSS", "Javascript", "Typescript"]
        };

        JobFilterResponseSimple res = await _jobfilterService.CreateNewJobFilter(req);
        JobFilterResponseDetail jobFilterDetail = await _jobfilterService.GetJobFilterDetail(res.Id);

        Assert.NotEqual(req.SoftSkills.Count, jobFilterDetail.SoftSkills!.Count);
    }

    [Fact]
    public async Task CreateNewJobFilter_RemoveDuplicateKeywordsInToolsField()
    {
        JobFilterCreationRequest? req = new()
        {
            FilterTitle = "Lap Trinh Vien Frontend",
            Languages = ["Vietnamese", "English"],
            Level = "fresher",
            YearsOfExperience = null,
            Occupation = "Information Technology",
            SoftSkills = ["Teamwork", "Presentation", "Communication"],
            TechnicalKnowledge = ["TCP/IP", "Web", "Frontend", "OOP"],
            Tools = ["HTML", "CSS", "Javascript", "Typescript", "Entity Framework", "entity framework"]
        };

        JobFilterResponseSimple res = await _jobfilterService.CreateNewJobFilter(req);
        JobFilterResponseDetail jobFilterDetail = await _jobfilterService.GetJobFilterDetail(res.Id);

        Assert.NotEqual(req.Tools.Count, jobFilterDetail.Tools!.Count);
    }

    [Fact]
    public async Task CreateNewJobFilter_RemoveDuplicateKeywordsInLanguageField()
    {
        JobFilterCreationRequest? req = new()
        {
            FilterTitle = "Lap Trinh Vien Frontend",
            Languages = ["Vietnamese", "English", "vietNAMesE"],
            Level = "fresher",
            YearsOfExperience = null,
            Occupation = "Information Technology",
            SoftSkills = ["Teamwork", "Presentation", "Communication"],
            TechnicalKnowledge = ["TCP/IP", "Web", "Frontend", "OOP"],
            Tools = ["HTML", "CSS", "Javascript", "Typescript"]
        };

        JobFilterResponseSimple res = await _jobfilterService.CreateNewJobFilter(req);
        JobFilterResponseDetail jobFilterDetail = await _jobfilterService.GetJobFilterDetail(res.Id);

        Assert.NotEqual(req.Languages.Count, jobFilterDetail.Languages!.Count);
    }

    [Fact]
    public async Task CreateNewJobFilter_RemoveDuplicateKeywordsInTechnicalKnowledgeField()
    {
        JobFilterCreationRequest? req = new()
        {
            FilterTitle = "Lap Trinh Vien Frontend",
            Languages = ["Vietnamese", "English"],
            Level = "fresher",
            YearsOfExperience = null,
            Occupation = "Information Technology",
            SoftSkills = ["Teamwork", "Presentation", "Communication"],
            TechnicalKnowledge = ["TCP/IP", "Web", "Frontend", "OOP", "oop"],
            Tools = ["HTML", "CSS", "Javascript", "Typescript"]
        };

        JobFilterResponseSimple res = await _jobfilterService.CreateNewJobFilter(req);
        JobFilterResponseDetail jobFilterDetail = await _jobfilterService.GetJobFilterDetail(res.Id);

        Assert.NotEqual(req.TechnicalKnowledge.Count, jobFilterDetail.TechnicalKnowledge!.Count);
    }

    [Fact]
    public async Task CreateNewJobFilter_MismatchBetweenFresherAndExpYear()
    {
        JobFilterCreationRequest? req = new()
        {
            FilterTitle = "Lap Trinh Vien Frontend",
            Languages = ["Vietnamese", "English"],
            Level = "fresher",
            YearsOfExperience = 1,
            Occupation = "Information Technology",
            SoftSkills = ["Teamwork", "Presentation", "Communication"],
            TechnicalKnowledge = ["TCP/IP", "Web", "Frontend", "OOP", "oop"],
            Tools = ["HTML", "CSS", "Javascript", "Typescript"]
        };

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
        {
            await _jobfilterService.CreateNewJobFilter(req);
        });
    }

    [Fact]
    public async Task CreateNewJobFilter_MatchBetweenJuniorAndExpYear()
    {
        JobFilterCreationRequest? req = new()
        {
            FilterTitle = "Lap Trinh Vien Frontend",
            Languages = ["Vietnamese", "English"],
            Level = "junior",
            YearsOfExperience = 3,
            Occupation = "Information Technology",
            SoftSkills = ["Teamwork", "Presentation", "Communication"],
            TechnicalKnowledge = ["TCP/IP", "Web", "Frontend", "OOP"],
            Tools = ["HTML", "CSS", "Javascript", "Typescript"]
        };

        var res = await _jobfilterService.CreateNewJobFilter(req);

        Assert.True(res.Id != Guid.Empty);
    }

    [Fact]
    public async Task CreateNewJobFilter_MismatchBetweenOtherLevelAndExpYear()
    {
        JobFilterCreationRequest? req = new()
        {
            FilterTitle = "Lap Trinh Vien Frontend",
            Languages = ["Vietnamese", "English"],
            Level = "senior",
            YearsOfExperience = 3,
            Occupation = "Information Technology",
            SoftSkills = ["Teamwork", "Presentation", "Communication"],
            TechnicalKnowledge = ["TCP/IP", "Web", "Frontend", "OOP"],
            Tools = ["HTML", "CSS", "Javascript", "Typescript"]
        };

        var res = await _jobfilterService.CreateNewJobFilter(req);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
        {
            await _jobfilterService.CreateNewJobFilter(req);
        });
    }

    [Fact]
    public async Task CreateNewJobFilter_IncorrectJobLevelFormat()
    {
        JobFilterCreationRequest? req = new()
        {
            FilterTitle = "Lap Trinh Vien Frontend",
            Languages = ["Vietnamese", "English"],
            Level = "Senior",
            YearsOfExperience = 3,
            Occupation = "Information Technology",
            SoftSkills = ["Teamwork", "Presentation", "Communication"],
            TechnicalKnowledge = ["TCP/IP", "Web", "Frontend", "OOP"],
            Tools = ["HTML", "CSS", "Javascript", "Typescript"]
        };

        var res = await _jobfilterService.CreateNewJobFilter(req);

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _jobfilterService.CreateNewJobFilter(req);
        });
    }

    [Fact]
    public async Task DeleteJobFilter_NonExistingJobFilterId()
    {
        Guid nonExistGuid = Guid.Parse("c02a25c6-ae15-441b-8a85-5d7360a49c15");
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _jobfilterService.DeleteJobFilter(nonExistGuid);
        });
    }
}
