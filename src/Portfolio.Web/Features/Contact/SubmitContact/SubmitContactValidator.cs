using FluentValidation;

namespace Portfolio.Web.Features.Contact.SubmitContact;

public sealed class SubmitContactValidator : AbstractValidator<SubmitContactRequest>
{
    public SubmitContactValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Adınızı yazmayı unutmuşsunuz.")
            .MinimumLength(2).WithMessage("Adınız en az 2 karakter olmalı.")
            .MaximumLength(150).WithMessage("Adınız 150 karakteri aşamaz.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta adresinizi yazmayı unutmuşsunuz.")
            .EmailAddress().WithMessage("Bu e-posta adresi geçerli görünmüyor.")
            .MaximumLength(320).WithMessage("E-posta adresi 320 karakteri aşamaz.");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Mesajınız boş görünüyor.")
            .MinimumLength(10).WithMessage("Mesajınız en az 10 karakter olmalı.")
            .MaximumLength(4000).WithMessage("Mesajınız 4000 karakteri aşamaz.");
    }
}
