using FluentValidation;

namespace Portfolio.Web.Features.Admin.Projects.SaveProject;

public sealed partial class SaveProjectValidator : AbstractValidator<SaveProjectRequest>
{
    public SaveProjectValidator()
    {
        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Slug zorunludur.")
            .MaximumLength(120)
            .Matches("^[a-z0-9]+(-[a-z0-9]+)*$").WithMessage("Slug yalnızca küçük harf, rakam ve tire içerebilir (örn: stok-takip-sistemi).");

        RuleFor(x => x.Title).NotEmpty().WithMessage("Başlık zorunludur.").MaximumLength(200);
        RuleFor(x => x.Summary).NotEmpty().WithMessage("Özet zorunludur.").MaximumLength(400);
        RuleFor(x => x.Problem).NotEmpty().WithMessage("Sorun alanı zorunludur.");
        RuleFor(x => x.Approach).NotEmpty().WithMessage("Yaklaşım alanı zorunludur.");
        RuleFor(x => x.Outcome).NotEmpty().WithMessage("Sonuç alanı zorunludur.");
        RuleFor(x => x.CoverImageUrl).MaximumLength(500);
        RuleFor(x => x.DemoUrl).MaximumLength(500);
        RuleFor(x => x.RepoUrl).MaximumLength(500);
    }
}
