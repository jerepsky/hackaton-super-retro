using FeedbackBros.SuperRetro;
using FeedbackBros.SuperRetro_1_0.Functionality.ServiceToSolution.AgentInPrivate.SuperRetro.Retrospective.DocumentWhatDidntGoWell_bundle.IWantToBeAbleToEditNegativeFeedback_group;
using FeedbackBros.SuperRetro_1_0.Functionality.ServiceToSolution.AgentInPrivate.SuperRetro.Retrospective.DocumentWhatDidntGoWell_bundle.IWantToBeAbleToRemoveNegativeFeedback_group;
using FeedbackBros.SuperRetro_1_0.Functionality.ServiceToSolution.AgentInPrivate.SuperRetro.Retrospective.DocumentWhatWentWell_bundle.IWantToBeAbleToAddPositiveFeedback_group;
using FeedbackBros.SuperRetro.SuperRetro;
using FeedbackBros.SuperRetro.SuperRetro.Retrospective;
using HackathonSuperRetro.Models;
using Uniscale.Core;

namespace HackathonSuperRetro.Interceptors;

public static class RetrospectiveFeedback
{
    public static void Setup(PlatformInterceptorBuilder builder, List<RetrospectiveData> retrospectives)
    {
        builder
            .InterceptRequest(NewFeedback.AllFeatureUsages, NewFeedback.Handler((input, _) =>
            {
                var retro = retrospectives.Find(r =>
                    r.RetrospectiveInstanceIdentifier == input.RetrospectiveIdentifier);
                if(retro == null)
                    return Result<Guid>.BadRequest(ErrorCodes.SuperRetro.NotFound);
                var newFeedback = new Feedback
                    {FeedbackIdentifier = Guid.NewGuid(), Content = input.Content, IsPositive = input.IsPositive};
                retro.Feedback.Add(newFeedback);
                
                return Result<Guid>.Ok(newFeedback.FeedbackIdentifier);
            }))
            .InterceptRequest(NullFeedback.AllFeatureUsages, NullFeedback.Handler((input, _) =>
            {
                var retro = retrospectives.Find(r =>
                    r.RetrospectiveInstanceIdentifier == input.RetrospectiveIdentifier);
                if(retro == null)
                    return Result<Empty>.BadRequest(ErrorCodes.SuperRetro.NotFound);
                
                var feedback = retro.Feedback.Find(f => 
                    f.FeedbackIdentifier == input.FeedbackIdentifier);
                if(feedback == null)
                    return Result<Empty>.BadRequest(ErrorCodes.SuperRetro.NotFound);
                feedback.Content = input.Content;
                feedback.IsPositive = input.IsPositive;
                
                return Result<Empty>.Ok(new Empty());
            }))
            .InterceptRequest(RemoveFeedback.AllFeatureUsages, RemoveFeedback.Handler((input, _) =>
            {
                var retro = retrospectives.Find(r =>
                    r.RetrospectiveInstanceIdentifier == input.RetrospectiveInstanceIdentifier);
                if(retro == null)
                    return Result<Empty>.BadRequest(ErrorCodes.SuperRetro.NotFound);
                
                var feedback = retro.Feedback.Find(f => 
                    f.FeedbackIdentifier == input.FeedbackIdentifier);
                if(feedback == null)
                    return Result<Empty>.BadRequest(ErrorCodes.SuperRetro.NotFound);
                
                retro.Feedback.Remove(feedback);
                return Result<Empty>.Ok(new Empty());
            }));
    }
}