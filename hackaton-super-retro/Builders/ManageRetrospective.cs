using FeedbackBros.SuperRetro;
using FeedbackBros.SuperRetro_1_0.Functionality.ServiceToSolution.AgentInPrivate.SuperRetro.ManageRetrospectives_bundle.IWantToBeAbleToAddNewRetrospective_group;
using FeedbackBros.SuperRetro_1_0.Functionality.ServiceToSolution.AgentInPrivate.SuperRetro.ManageRetrospectives_bundle.IWantToBeAbleToRemoveRetrospective_group;
using FeedbackBros.SuperRetro_1_0.Functionality.ServiceToSolution.AgentInPrivate.SuperRetro.ManageRetrospectives_bundle.IWantToBeAbleToUpdateRetrospective_group;
using FeedbackBros.SuperRetro.SuperRetro;
using hackaton_super_retro.Models;
using Uniscale.Core;

namespace hackaton_super_retro.Builders;

public static class ManageRetrospective
{
    public static void Setup(PlatformInterceptorBuilder builder, List<RetrospectiveData> retrospectives)
    {
        builder
            .InterceptRequest(NewRetrospective.AllFeatureUsages, NewRetrospective.Handler((input, _) =>
            {
                var newRetro = new RetrospectiveData
                    { RetrospectiveInstanceIdentifier = Guid.NewGuid(), Name = input.Name };
                retrospectives.Add(newRetro);
                return newRetro.RetrospectiveInstanceIdentifier;
            }))
            .InterceptRequest(CreateOrReplaceRetrospective.AllFeatureUsages, CreateOrReplaceRetrospective.Handler((input, _) =>
            {
                var newRetro = new RetrospectiveData
                    { Name = input.Name, Attendees = input.Attendees };
                retrospectives.Add(newRetro);
                return newRetro.RetrospectiveInstanceIdentifier;
            }))
            .InterceptRequest(NullRestrospective.AllFeatureUsages, NullRestrospective.Handler((input, _) =>
            {
                var retro = retrospectives.Find(r => 
                    r.RetrospectiveInstanceIdentifier == input.RetrospectiveInstanceIdentifier);
                if(retro == null)
                    return Result<Empty>.BadRequest(ErrorCodes.SuperRetro.NotFound);
                retro.Name = input.Name;
                return Result<Empty>.Ok(new Empty());
            }))
            .InterceptRequest(ReplaceRetrospective.AllFeatureUsages, ReplaceRetrospective.Handler((input, _) =>
            {
                var retro = retrospectives.Find(r => 
                    r.RetrospectiveInstanceIdentifier == input.RetrospectiveInstanceIdentifier);
                if(retro != null)
                    retrospectives.Remove(retro);
                
                retrospectives.Add(new RetrospectiveData
                    { RetrospectiveInstanceIdentifier = Guid.NewGuid(), Name = input.Name, Attendees = input.Attendees });
                
                return new Empty();
            }))
            .InterceptRequest(DeactivateRetrospective.AllFeatureUsages, DeactivateRetrospective.Handler((input, _) =>
            {
                var retro = retrospectives.Find(r => 
                    r.RetrospectiveInstanceIdentifier == input);
                if (retro == null)
                    return Result<Empty>.BadRequest(ErrorCodes.SuperRetro.NotFound);
                
                retrospectives.Remove(retro);
                return Result<Empty>.Ok(new Empty());
            }));
    }
}