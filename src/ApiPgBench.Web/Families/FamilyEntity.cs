using ApiPgBench.Web.Storages;

namespace ApiPgBench.Web.Families;

public class FamilyEntity : IEntity
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public FamilyViewModel ToViewModel()
    {
        return new() { Id = Id, Name = Name };
    }

    public static FamilyEntity From(FamilyViewModel viewModel)
    {
        return new() { Id = viewModel.Id, Name = viewModel.Name };
    }
}
