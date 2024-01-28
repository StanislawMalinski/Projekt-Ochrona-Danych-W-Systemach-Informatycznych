using projekt.Models.Dtos;

namespace projekt.Services.Interfaces;
public interface IAccessService
{
    bool ShouldReplay(string origin);
}
