using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Components;

public class SectionsViewComponent : ViewComponent
{
    private readonly IProductData _productData;

    public SectionsViewComponent(IProductData productData) => _productData = productData;

    public IViewComponentResult Invoke()
    {
        var sections = _productData.GetSections();
        var parentSections = sections.Where(s => s.ParentId is null);
        var parentSectionViewModels = parentSections.Select(ps =>
            new SectionViewModel()
            {
                Id = ps.Id,
                Name = ps.Name,
                Order = ps.Order,
            })
            .ToList();

        foreach (var parentSection in parentSectionViewModels)
        {
            var childSections = sections.Where(s => s.ParentId == parentSection.Id).ToList();

            foreach (var section in childSections)
            {
                parentSection.ChildSections.Add(new SectionViewModel()
                {
                    Id = section.Id,
                    Name = section.Name,
                    Order = section.Order,
                });
            }

            parentSection.ChildSections.Sort((a,b) => Comparer<int>.Default.Compare(a.Order, b.Order));
        }
        return View(parentSectionViewModels);
    }
}