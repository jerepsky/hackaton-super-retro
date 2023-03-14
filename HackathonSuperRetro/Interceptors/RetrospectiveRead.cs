using FeedbackBros.SuperRetro;
using FeedbackBros.SuperRetro_1_0.Functionality.ServiceToSolution.AgentInPrivate.SuperRetro.Retrospective.ViewRetrospective_bundle.IWantToGetRetrospectiveInfo_group;
using FeedbackBros.SuperRetro.SuperRetro;
using FeedbackBros.SuperRetro.SuperRetro.Retrospective;
using HackathonSuperRetro.Models;
using Uniscale.Core;
using Action = FeedbackBros.SuperRetro.SuperRetro.Retrospective.Action;

namespace HackathonSuperRetro.Interceptors;

public static class RetrospectiveRead
{
    public static void Setup(PlatformInterceptorBuilder builder, List<RetrospectiveData> retrospectives, List<User> users)
    {
        builder
            .InterceptRequest(GetAttendees.AllFeatureUsages, GetAttendees.Handler((input, _) =>
            {
                var retro = retrospectives.Find(r =>
                    r.RetrospectiveInstanceIdentifier == input);
                if(retro == null)
                    return Result<List<User>>.BadRequest(ErrorCodes.SuperRetro.NotFound);

                var attendeeData = users.FindAll(u => retro.Attendees.Contains(u.UserIdentifier));
                
                return Result<List<User>>.Ok(attendeeData);
            }))
            .InterceptRequest(GetFeedbackList.AllFeatureUsages, GetFeedbackList.Handler((input, _) =>
            {
                var retro = retrospectives.Find(r =>
                    r.RetrospectiveInstanceIdentifier == input);
                if(retro == null)
                    return Result<List<Feedback>>.BadRequest(ErrorCodes.SuperRetro.NotFound);
                
                return Result<List<Feedback>>.Ok(retro.Feedback);
            }))
            .InterceptRequest(GetImprovementIdeas.AllFeatureUsages, GetImprovementIdeas.Handler((input, _) =>
            {
                var retro = retrospectives.Find(r =>
                    r.RetrospectiveInstanceIdentifier == input);
                if(retro == null)
                    return Result<List<ImprovementIdea>>.BadRequest(ErrorCodes.SuperRetro.NotFound);
                
                return Result<List<ImprovementIdea>>.Ok(retro.ImprovementIdeas);
            }))
            .InterceptRequest(GetActions.AllFeatureUsages, GetActions.Handler((input, _) =>
            {
                var retro = retrospectives.Find(r =>
                    r.RetrospectiveInstanceIdentifier == input);
                if(retro == null)
                    return Result<List<Action>>.BadRequest(ErrorCodes.SuperRetro.NotFound);
                
                return Result<List<Action>>.Ok(retro.Actions);
            }))
            .InterceptRequest(GetRetrospective.AllFeatureUsages, GetRetrospective.Handler((input, _) =>
            {
                var retro = retrospectives.Find(r =>
                    r.RetrospectiveInstanceIdentifier == input);
                if(retro == null)
                    return Result<RetrospectiveResponse>.BadRequest(ErrorCodes.SuperRetro.NotFound);

                var ret = new RetrospectiveResponse
                {
                    RetrospectiveIdentifier = retro.RetrospectiveInstanceIdentifier,
                    Name = retro.Name,
                    Action = retro.Actions.Select(a => a.ActionIdentifier).ToList(),
                    Feedback = retro.Feedback.Select(f => f.FeedbackIdentifier).ToList(),
                    ImprovementIdeas = retro.ImprovementIdeas.Select(i => i.ImprovementIdeaIdentifier).ToList(),
                    Attendees = retro.Attendees
                };
                
                return Result<RetrospectiveResponse>.Ok(ret);
            }));
    }
}