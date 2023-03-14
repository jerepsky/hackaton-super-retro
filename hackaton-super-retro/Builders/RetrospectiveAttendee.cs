using FeedbackBros.SuperRetro;
using FeedbackBros.SuperRetro_1_0.Functionality.ServiceToSolution.AgentInPrivate.SuperRetro.Retrospective.DocumentAttendees_bundle.IWantToBeAbleToAddAttendee_group;
using FeedbackBros.SuperRetro_1_0.Functionality.ServiceToSolution.AgentInPrivate.SuperRetro.Retrospective.DocumentAttendees_bundle.IWantToBeAbleToAddAttendee_group.SuperRetro;
using FeedbackBros.SuperRetro_1_0.Functionality.ServiceToSolution.AgentInPrivate.SuperRetro.Retrospective.DocumentAttendees_bundle.IWantToBeAbleToRemoveAttendee_group;
using FeedbackBros.SuperRetro_1_0.Functionality.ServiceToSolution.AgentInPrivate.SuperRetro.Retrospective.DocumentWhatDidntGoWell_bundle.IWantToBeAbleToEditNegativeFeedback_group;
using FeedbackBros.SuperRetro_1_0.Functionality.ServiceToSolution.AgentInPrivate.SuperRetro.Retrospective.DocumentWhatDidntGoWell_bundle.IWantToBeAbleToRemoveNegativeFeedback_group;
using FeedbackBros.SuperRetro_1_0.Functionality.ServiceToSolution.AgentInPrivate.SuperRetro.Retrospective.DocumentWhatWentWell_bundle.IWantToBeAbleToAddPositiveFeedback_group;
using FeedbackBros.SuperRetro_1_0.Functionality.ServiceToSolution.AgentInPrivate.SuperRetro.Retrospective.WhatActionsWillFollow_bundle.IWantToBeAbleToAddActions_group;
using FeedbackBros.SuperRetro_1_0.Functionality.ServiceToSolution.AgentInPrivate.SuperRetro.Retrospective.WhatActionsWillFollow_bundle.IWantToBeAbleToDeleteActions_group;
using FeedbackBros.SuperRetro_1_0.Functionality.ServiceToSolution.AgentInPrivate.SuperRetro.Retrospective.WhatActionsWillFollow_bundle.IWantToBeAbleToUpdateActions_group;
using FeedbackBros.SuperRetro.SuperRetro;
using FeedbackBros.SuperRetro.SuperRetro.Retrospective;
using Uniscale.Core;
using Action = FeedbackBros.SuperRetro.SuperRetro.Retrospective.Action;

namespace hackaton_super_retro.Builders;

public static class RetrospectiveAttendee
{
    public static void Setup(PlatformInterceptorBuilder builder, List<RetrospectiveData> retrospectives, List<User> users)
    {
        builder
            .InterceptRequest(NewAttendee.AllFeatureUsages, NewAttendee.Handler((input, _) =>
            {
                var retro = retrospectives.Find(r =>
                    r.RetrospectiveInstanceIdentifier == input.RetrospectiveInstanceIdentifier);
                if(retro == null)
                    return Result<Empty>.BadRequest(ErrorCodes.SuperRetro.NotFound);
                var user = users.Find(u => u.UserIdentifier == input.UserIdentifier);
                if(user == null)
                    return Result<Empty>.BadRequest(ErrorCodes.SuperRetro.NotFound);
                
                retro.Attendees.Add(user.UserIdentifier);
                
                return Result<Empty>.Ok(new Empty());
            }))
            .InterceptRequest(ReplaceAttendeees.AllFeatureUsages, ReplaceAttendeees.Handler((input, _) =>
            {
                var retro = retrospectives.Find(r =>
                    r.RetrospectiveInstanceIdentifier == input.RetrospectiveInstanceIdentifier);
                if(retro == null)
                    return Result<Empty>.BadRequest(ErrorCodes.SuperRetro.NotFound);
                retro.Attendees = input.Attendees;

                return Result<Empty>.Ok(new Empty());
            }))
            .InterceptRequest(RemoveAttendee.AllFeatureUsages, RemoveAttendee.Handler((input, _) =>
            {
                var retro = retrospectives.Find(r =>
                    r.RetrospectiveInstanceIdentifier == input.RetrospectiveInstanceIdentifier);
                if(retro == null)
                    return Result<Empty>.BadRequest(ErrorCodes.SuperRetro.NotFound);

                var success= retro.Attendees.Remove(input.UserIdentifier);
                return success ? Result<Empty>.BadRequest(ErrorCodes.SuperRetro.NotFound) : Result<Empty>.Ok(new Empty());
            }));
    }
}