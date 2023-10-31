public class RandomQuestion
{
    public static QuestionModel GetRandomQuestion()
    {
        List<QuestionModel> questions_with_answers = QuestionsWithAnswersRepository.GetQuestionsWithAnswers();

        Random random = new Random();
        int randomIndex = random.Next(0, questions_with_answers.Count);


        QuestionModel RandomQuestion = questions_with_answers[randomIndex];

        return RandomQuestion;


    }
}