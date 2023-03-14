using FeedbackBros.SuperRetro;
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

public static class RetrospectiveAction
{
    public static void Setup(PlatformInterceptorBuilder builder, List<RetrospectiveData> retrospectives)
    {
        builder
            .InterceptRequest(NewAction.AllFeatureUsages, NewAction.Handler((input, _) =>
            {
                var retro = retrospectives.Find(r =>
                    r.RetrospectiveInstanceIdentifier == input.RetrospectiveIdentifier);
                if(retro == null)
                    return Result<Guid>.BadRequest(ErrorCodes.SuperRetro.NotFound);
                var newAction = new Action
                    { ActionIdentifier= Guid.NewGuid(), Content = input.Content, Deadline = input.Deadline, RetrospectiveIdentifier = retro.RetrospectiveInstanceIdentifier};
                retro.Actions.Add(newAction);
                
                return Result<Guid>.Ok(newAction.ActionIdentifier);
            }))
            .InterceptRequest(NullAction.AllFeatureUsages, NullAction.Handler((input, _) =>
            {
                var retro = retrospectives.Find(r =>
                    r.RetrospectiveInstanceIdentifier == input.RetrospectiveIdentifier);
                if(retro == null)
                    return Result<Empty>.BadRequest(ErrorCodes.SuperRetro.NotFound);
                
                var action = retro.Actions.Find(f => 
                    f.ActionIdentifier == input.ActionIdentifier);
                if(action == null)
                    return Result<Empty>.BadRequest(ErrorCodes.SuperRetro.NotFound);
                action.Content = input.Content;
                action.Deadline = input.Deadline;
                
                return Result<Empty>.Ok(new Empty());
            }))
            /*.InterceptRequest(RemoveAction.AllFeatureUsages, RemoveAction.Handler((input, _) =>
            {
                var retro = retrospectives.Find(r =>
                    r.RetrospectiveInstanceIdentifier == input.RetrospectiveIdentifier);
                if(retro == null)
                    return Result<Empty>.BadRequest(ErrorCodes.SuperRetro.NotFound);
                
                var feedback = retro.Actions.Find(f => 
                    f.ActionIdentifier == input.ActionIdentifier);
                if(feedback == null)
                    return Result<Empty>.BadRequest(ErrorCodes.SuperRetro.NotFound);
                
                retro.Actions.Remove(feedback);
                return Result<Empty>.Ok(new Empty());
            }))*/;
    }
}