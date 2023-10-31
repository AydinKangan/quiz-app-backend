public class QuestionsWithAnswersRepository
{
    public static List<QuestionModel> GetQuestionsWithAnswers()
    {
        List<QuestionModel> questions = QuestionRepository.GetAllQuestions();
        List<AnswerModel> answers = AnswerRepository.GetAnswers();

        foreach (AnswerModel answer in answers)
        {
            int? question_id = answer.QuestionId;
            QuestionModel? question = questions.Find(c => c.Id == question_id);

            if (question != null)
            {
                question.Answers ??= new List<AnswerModel>();
                question.Answers.Add(answer);
            }
        }

        return questions;
    }


}