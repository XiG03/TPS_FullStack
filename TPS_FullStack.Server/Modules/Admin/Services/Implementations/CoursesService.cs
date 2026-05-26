using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Identity.Client;
using TPS_FullStack.Server.Entities;

namespace TPS_FullStack.Server.Modules.Admin
{
    public class CoursesService : ICoursesService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ILogger<AppUser> _logger;
        public CoursesService(ICourseRepository courseRepository, ILogger<AppUser> logger)
        {
            _courseRepository = courseRepository;
            _logger = logger;
        }
        public async Task<bool> CourseDeleteByIdAsync(string MaID)
        {
            var result = await _courseRepository.CourseDeleteByIdAsync(MaID);
            if (!result)
            {
                return false;
            }
            return true;
            throw new NotImplementedException();
        }

        public async Task<List<CourseInfo>> CourseGetAllAsync()
        {
            var result = await _courseRepository.CourseGetAllAsync();
            if (result == null)
            {
                return null;
            }
            return result;
            throw new NotImplementedException();
        }

        public async Task<CourseDetailDto> CourseGetByIdAsync(string MaID)
        {

            var result = await _courseRepository.CourseGetByIdAsync(MaID);
            if (result == null)
            {
                return null;
            }
            return result;
            throw new NotImplementedException();
        }

        private List<CourseScheduleDto> createSchedules(List<CourseScheduleDto> schedules, List<int> studyDays, DateTime Ngayhientai, decimal Sobuoihoc, int count)
        {
            if (count >= Sobuoihoc)
            {
                return schedules;
            }
            // logic tinh ngay tiep theo va format kiem tra thang
            schedules.Add(new CourseScheduleDto
            {
                MaID = Guid.NewGuid().ToString(),
                Ngaydukien = Ngayhientai,
            });
            // logger de track - se duoc xoa neu dung yeu cau
            _logger.LogInformation(schedules.ToString());

            int loca = count % studyDays.Count();
            DateTime Ngaytieptheo = Ngayhientai.AddDays(loca == studyDays.Count() - 1 ? studyDays[0] - studyDays[loca] + 7
                                                                            : studyDays[loca + 1] - studyDays[loca]);
            return createSchedules(schedules, studyDays, Ngaytieptheo, Sobuoihoc, ++count);

        }

        public async Task<CourseCreateDto> CourseInsertAsync(CourseCreateDto createDto)
        {
            if (createDto.courseDto.Thu != null)
            {
                try
                {
                    // Tach chuoi thu va don no thanh list int
                    var studyDays = new List<int>();
                    foreach (var item in createDto.courseDto.Thu.Split("#"))
                    {
                        if (int.TryParse(item, out int number))
                        {
                            studyDays.Add(number);
                        }
                    }

                    if (studyDays.Count == 0) return null;
                    List<CourseScheduleDto> list = new List<CourseScheduleDto>();
                    list = this.createSchedules(list, studyDays, createDto.courseDto.Ngaybatdau, createDto.courseDto.Sobuoihoc, 0);
                    createDto.courseScheduleDtos = list;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return null;
                }
            }
            var result = await _courseRepository.CourseInsertAsync(createDto);
            if (!result)
            {
                return null;
            }
            return createDto;
            throw new NotImplementedException();
        }
        // Kiem tra, xu ly lai
        public async Task<CourseUpdateDto> CourseUpdateAsync(CourseUpdateDto updateDto)
        {
            try
            {
                if (updateDto.courseUpdate.Ngaybatdau == null)
                {
                    return null;
                }
                var studyDays = new List<int>();
                foreach (var item in updateDto.courseUpdate.Thu.Split("#"))
                {
                    if (int.TryParse(item, out int number))
                    {
                        studyDays.Add(number);
                    }
                }
                if (studyDays.Count == 0) return null;
                List<CourseScheduleDto> list = new List<CourseScheduleDto>();
                list = this.createSchedules(list, studyDays, updateDto.courseUpdate.Ngaybatdau, updateDto.courseUpdate.Sobuoihoc, 0);
                updateDto.courseScheduleUpdates = list;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
            var result = await _courseRepository.CourseUpdateAsync(updateDto);
            if (!result)
            {
                return null;
            }
            return updateDto;
            throw new NotImplementedException();
        }

        public async Task<ServiceDefault<updateStudentScoreDto>> updateStudentScoreAsync(updateStudentScoreDto updateDto)
        {
            var result = await _courseRepository.UpdateStudentScoreAsync(updateDto);

            if (!result)
            {
                return new ServiceDefault<updateStudentScoreDto>
                {
                    statusCode = StatusCodes.Status500InternalServerError,
                    Message = "Server can't update",
                    Data = null
                };
            }
            if (result)
            {
                return new ServiceDefault<updateStudentScoreDto>
                {
                    statusCode = StatusCodes.Status200OK,
                    Message = "Complete to update student's score",
                    Data = updateDto
                };
            }

            throw new NotImplementedException();
        }
    }

}

