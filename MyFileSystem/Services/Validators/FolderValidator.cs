using FluentValidation;
using MyFileSystem.Core.DTOs.FileManager.FolderDtos;

namespace MyFileSystem.Services.Validators
{
    public class CreateFolderValidator : AbstractValidator<CreateFolderDto>
    {
        public CreateFolderValidator()
        {
            RuleFor(f => f.FolderName).NotEmpty().MinimumLength(1).MaximumLength(20).WithMessage("Folder Name Must be filled. ");
        }
    }
}
