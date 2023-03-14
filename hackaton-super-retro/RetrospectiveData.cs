using FeedbackBros.SuperRetro.SuperRetro.Retrospective;
using Action = FeedbackBros.SuperRetro.SuperRetro.Retrospective.Action;

namespace hackaton_super_retro;

public class RetrospectiveData : RetrospectiveInstance
{
    public readonly List<Feedback> Feedback = new List<Feedback>();
    public readonly List<ImprovementIdea> ImprovementIdeas = new List<ImprovementIdea>();
    public readonly List<Action> Actions = new List<Action>();
}