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
                throw new Exception("Không tìm thấy câu hỏi nào cho khóa học này.");
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
    }
}
