using FeedbackBros.SuperRetro.SuperRetro.Retrospective;
using Action = FeedbackBros.SuperRetro.SuperRetro.Retrospective.Action;

namespace HackathonSuperRetro.Models;

public class RetrospectiveData : RetrospectiveInstance
{
    public readonly List<Feedback> Feedback = new();
    public readonly List<ImprovementIdea> ImprovementIdeas = new();
    public readonly List<Action> Actions = new();
}