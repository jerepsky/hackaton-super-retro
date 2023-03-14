using FeedbackBros.SuperRetro;
using FeedbackBros.SuperRetro_1_0.Functionality.ServiceToSolution.AgentInPrivate.SuperRetro.Retrospective.WhatCanWeImprove_bundle.IWantToBeAbleToAddImprovementIdeas_group;
using FeedbackBros.SuperRetro_1_0.Functionality.ServiceToSolution.AgentInPrivate.SuperRetro.Retrospective.WhatCanWeImprove_bundle.IWantToBeAbleToEditImprovementIdeas_group;
using FeedbackBros.SuperRetro_1_0.Functionality.ServiceToSolution.AgentInPrivate.SuperRetro.Retrospective.WhatCanWeImprove_bundle.IWantToBeAbleToRemoveImprovementIdeas_group;
using FeedbackBros.SuperRetro.SuperRetro;
using FeedbackBros.SuperRetro.SuperRetro.Retrospective;
using hackaton_super_retro.Models;
using Uniscale.Core;

namespace hackaton_super_retro.Builders;

public static class RetrospectiveImprovementIdea
{
    public static void Setup(PlatformInterceptorBuilder builder, List<RetrospectiveData> retrospectives)
    {
        builder
            .InterceptRequest(NewImprovementIdea.AllFeatureUsages, NewImprovementIdea.Handler((input, _) =>
            {
                var retro = retrospectives.Find(r =>
                    r.RetrospectiveInstanceIdentifier == input.RetrospectiveIdentifier);
                if(retro == null)
                    return Result<Guid>.BadRequest(ErrorCodes.SuperRetro.NotFound);
                var newIdea= new ImprovementIdea
                    {ImprovementIdeaIdentifier = Guid.NewGuid(), Content = input.Content, RetrospectiveIdentifier = retro.RetrospectiveInstanceIdentifier};
                retro.ImprovementIdeas.Add(newIdea);
                
                return Result<Guid>.Ok(newIdea.ImprovementIdeaIdentifier);
            }))
            .InterceptRequest(NullImprovementIdea.AllFeatureUsages, NullImprovementIdea.Handler((input, _) =>
            {
                var retro = retrospectives.Find(r =>
                    r.RetrospectiveInstanceIdentifier == input.RetrospectiveIdentifier);
                if(retro == null)
                    return Result<Empty>.BadRequest(ErrorCodes.SuperRetro.NotFound);
                
                var idea = retro.ImprovementIdeas.Find(f => 
                    f.ImprovementIdeaIdentifier == input.ImprovementIdeaIdentifier);
                if(idea == null)
                    return Result<Empty>.BadRequest(ErrorCodes.SuperRetro.NotFound);
                idea.Content = input.Content;

                return Result<Empty>.Ok(new Empty());
            }))
            .InterceptRequest(RemoveImprovementIdea.AllFeatureUsages, RemoveImprovementIdea.Handler((input, _) =>
            {
                var retro = retrospectives.Find(r =>
                    r.RetrospectiveInstanceIdentifier == input.RetrospectiveInstanceIdentifier);
                if(retro == null)
                    return Result<Empty>.BadRequest(ErrorCodes.SuperRetro.NotFound);
                
                var idea = retro.ImprovementIdeas.Find(f => 
                    f.ImprovementIdeaIdentifier == input.ImprovementIdeaIdentifier);
                if(idea == null)
                    return Result<Empty>.BadRequest(ErrorCodes.SuperRetro.NotFound);
                
                retro.ImprovementIdeas.Remove(idea);
                return Result<Empty>.Ok(new Empty());
            }));
    }
}