using FeedbackBros.SuperRetro;
using FeedbackBros.SuperRetro_1_0.Functionality.ServiceToSolution.AgentInPrivate.SuperRetro.ManageRetrospectives_bundle.IWantToBeAbleToRemoveRetrospective_group;
using FeedbackBros.SuperRetro_1_0.Functionality.ServiceToSolution.AgentInPrivate.SuperRetro.ManageUsers_bundle.IWantToBeAbleToAddNewUser_group;
using FeedbackBros.SuperRetro_1_0.Functionality.ServiceToSolution.AgentInPrivate.SuperRetro.ManageUsers_bundle.IWantToBeAbleToUpdateAttendees_group;
using FeedbackBros.SuperRetro.SuperRetro;
using FeedbackBros.SuperRetro.SuperRetro.Retrospective;
using Uniscale.Core;

namespace HackathonSuperRetro.Interceptors;

public static class ManageUser
{
    public static void Setup(PlatformInterceptorBuilder builder, List<User> users)
    {
        builder
            .InterceptRequest(NewUser.AllFeatureUsages, NewUser.Handler((input, _) =>
            {
                var newUser = new User
                    { UserIdentifier = Guid.NewGuid(), Name = input.Name, Email = input.Email };
                users.Add(newUser);
                return newUser.UserIdentifier;
            }))
            .InterceptRequest(NullUser.AllFeatureUsages, NullUser.Handler((input, _) =>
            {
                var user = users.Find(r => 
                    r.UserIdentifier == input.UserIdentifier);
                if(user == null)
                    return Result<Empty>.BadRequest(ErrorCodes.SuperRetro.NotFound);
                user.Name = input.Name;
                return Result<Empty>.Ok(new Empty());
            }))
            .InterceptRequest(DeactivateRetrospective.AllFeatureUsages, DeactivateRetrospective.Handler((input, _) =>
            {
                var retro = users.Find(r => 
                    r.UserIdentifier == input);
                if (retro == null)
                    return Result<Empty>.BadRequest(ErrorCodes.SuperRetro.NotFound);
                
                users.Remove(retro);
                return Result<Empty>.Ok(new Empty());
            }));
    }
}