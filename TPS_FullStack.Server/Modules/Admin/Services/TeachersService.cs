using Microsoft.Identity.Client;
using Microsoft.JSInterop.Infrastructure;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class TeachersService : ITeachersService
    {
        private readonly ITeachersRepository _teachersRepository;
        
        public TeachersService(ITeachersRepository teachersRepository)
        {
            _teachersRepository = teachersRepository;
        }

        public async Task<TeacherCreateDto> CreateTeacherAsync(TeacherCreateDto createDto)
        {
            var result = await _teachersRepository.CreateTeacherAsync(createDto);
            if (result)
            {
                return createDto;
            }
            return null;
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteTeacherAsync(string MaID)
        {
            var result = await _teachersRepository.DeleteTeacherAsync(MaID);
            if (result)
            {
                return true;
            }
            return false;
            throw new NotImplementedException();
        }

        public async Task<List<TeacherGetAllDto>> TeacherGetAllAsync()
        {
            var data = await _teachersRepository.TeacherGetAllAsync();
            if(data == null)
            {
                return null;
            }
            return data;
            throw new NotImplementedException();
        }
    }
}

