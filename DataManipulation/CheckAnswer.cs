
public class CheckAnswer
{
    public static List<CheckedModel> CheckAnswers(ToCheckModel[] CheckAnswers, int userId)
    {
        List<CheckedModel> checkedAnswers = new List<CheckedModel>();
        int correctAnswers = 0;

        foreach (ToCheckModel CheckAnswer in CheckAnswers)
        {
            int questionId = CheckAnswer.QuestionId;
            string? chosenAnswerId = CheckAnswer.AnswerId;

            string? correctAnswerId = QuestionRepository.GetCorrectAnswerId(questionId);

            bool isCorrect = chosenAnswerId == correctAnswerId;

            if (isCorrect == true)
            {
                correctAnswers += 1;
            }

            checkedAnswers.Add(new CheckedModel(question: CheckAnswer.Question, isCorrect: isCorrect));
        }
        int totalAnswers = CheckAnswers.Length;
        UserRepository.UpdateUserStats(userId, correctAnswers);
        ResultsRepository.InsertResults(userId, correctAnswers, totalAnswers);

        return checkedAnswers;

    }

}