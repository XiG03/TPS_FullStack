namespace TPS_FullStack.Server.Modules.Admin
{
    public class FinalExamServices : IFinalExamServices
    {
        private readonly IFinalExamRepository _finalExamRepository;

        public FinalExamServices(IFinalExamRepository finalExamRepository)
        {
            _finalExamRepository = finalExamRepository;
        }

        public async Task<ServiceDefault<List<finalExams>>> GenerateFinalExamAsync(List<string> HocvienIDs, string KhoahocID, decimal Socauhoi)
        {
            var examQuestions = await _finalExamRepository.GetExamQuestions(KhoahocID);
            if (examQuestions == null)
            {
                return new ServiceDefault<List<finalExams>>
                { statusCode = 404, Message = "Không tìm thấy câu hỏi nào cho khóa học này.", Data = null };
            }
            var finalExamsList = new List<finalExams>();
            Random rnd = new Random();

            foreach (var hocvienID in HocvienIDs)
            {
                var randomizedQuestions = new List<examQuestions>(examQuestions);

                for (int i = randomizedQuestions.Count - 1; i > 0; i--)
                {
                    int j = rnd.Next(0, i + 1);
                    var temp = randomizedQuestions[i];
                    randomizedQuestions[i] = randomizedQuestions[j];
                    randomizedQuestions[j] = temp;
                }
                if ((int)Socauhoi > randomizedQuestions.Count)
                {
                    return new ServiceDefault<List<finalExams>>
                    { statusCode = 400, Message = "Số lượng câu hỏi yêu cầu vượt quá ngân hàng câu hỏi hiện có.", Data = null };
                }
                var selectedQuestions = randomizedQuestions.GetRange(0, (int)Socauhoi);
                var finalExam = new finalExams
                {
                    MaID = Guid.NewGuid().ToString(),
                    KhoahocID = KhoahocID,
                    HocvienID = hocvienID,
                    Thoigianlambai = 60,
                    Danhsachcauhoi = new List<finalExamQuestions>()
                };

                foreach (var question in selectedQuestions)
                {
                    finalExam.Danhsachcauhoi.Add(new finalExamQuestions
                    {
                        MaID = Guid.NewGuid().ToString(),
                        BaithuhoachID = finalExam.MaID,
                        KhoahocID = KhoahocID,
                        ChuyendeID = question.ChuyendeID,
                        CauhoiID = question.CauhoiID,
                        Tencauhoi = question.Tencauhoi,
                        DapanID = question.DapanID,
                        Noidungdapan = question.Noidungdapan
                    });
                }
                finalExamsList.Add(finalExam);
            }
            var result = await _finalExamRepository.FinalExamInsertAsync(finalExamsList);
            if (!result)
            {
                return new ServiceDefault<List<finalExams>>
                { statusCode = 500, Message = "Đã có lỗi xảy ra khi lưu đề thi cuối khóa vào cơ sở dữ liệu.", Data = null };
            }


            return new ServiceDefault<List<finalExams>>
            { statusCode = 200, Message = "Đề thi cuối khóa đã được tạo thành công.", Data = finalExamsList };
        }

        public async Task<ServiceDefault<finalExamDetail>> GetFinalExamDetailAsync(string MaID)
        {
            var detail = await _finalExamRepository.GetFinalExamDetailAsync(MaID);
            if (detail == null)
            {
                return new ServiceDefault<finalExamDetail>
                { statusCode = 404, Message = "Không tìm thấy chi tiết đề thi cuối khóa.", Data = null };
            }
            return new ServiceDefault<finalExamDetail>
            { statusCode = 200, Message = "Tìm thấy chi tiết đề thi cuối khóa.", Data = detail };
        }

        public async Task<ServiceDefault<List<finalExamList>>> GetFinalExamListsAsync()
        {
            var list = await _finalExamRepository.GetFinalExamListsAsync();
            if (list == null)
            {
                return new ServiceDefault<List<finalExamList>>
                { statusCode = 404, Message = "Không tìm thấy đề thi cuối khóa nào.", Data = null };
            }
            return new ServiceDefault<List<finalExamList>>
            { statusCode = 200, Message = "Tìm thấy đề thi cuối khóa.", Data = list };
        }

        public async Task<ServiceDefault<List<finalExamList>>> GetFinalExamListsAsync(string KhoahocID)
        {
            var list = await _finalExamRepository.GetFinalExamListsAsync(KhoahocID);
            if (list == null)
            {
                return new ServiceDefault<List<finalExamList>>
                { statusCode = 404, Message = "Không tìm thấy đề thi cuối khóa nào.", Data = null };
            }
            return new ServiceDefault<List<finalExamList>>
            { statusCode = 200, Message = "Tìm thấy đề thi cuối khóa.", Data = list };
        }

        public async Task<ServiceDefault<bool>> UpdateFinalExamScoreAsync(finalExamUpdate finalExamUpdate)
        {
            var result = await _finalExamRepository.UpdateFinalExamScoreAsync(finalExamUpdate);
            if (!result)
            {
                return new ServiceDefault<bool>
                { statusCode = 500, Message = "Đã có lỗi xảy ra khi cập nhật điểm thi cuối khóa.", Data = false };
            }
            return new ServiceDefault<bool>
            { statusCode = 200, Message = "Điểm thi cuối khóa đã được cập nhật thành công.", Data = true };
        }
    }
}
