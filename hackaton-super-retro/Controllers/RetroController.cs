using System.Net;
using FeedbackBros.SuperRetro_1_0.Functionality.ServiceToSolution.AgentInPrivate.SuperRetro.ManageRetrospectives_bundle.IWantToBeAbleToAddNewRetrospective_group;
using FeedbackBros.SuperRetro.SuperRetro.Retrospective;
using Microsoft.AspNetCore.Mvc;
using Uniscale.Core;
using Uniscale.Designtime;

namespace hackaton_super_retro.Controllers;

[ApiController]
[Route("[controller]")]
public class RetroController : ControllerBase
{
    private readonly List<RetrospectiveInstance> _retrospectives;
    private readonly ILogger<RetroController> _logger;

    public RetroController(ILogger<RetroController> logger)
    {
        _logger = logger;
        _retrospectives = new List<RetrospectiveInstance>();
    }

    [HttpPost(Name = "Request")]
    public async Task<Result> Post([FromBody] string request)
    {
        var session = await Platform.Builder()
            // Set up an interceptor for the feature that returns a new task from the input
            .WithInterceptors(i => i
                .InterceptRequest(NewRetrospective.AllFeatureUsages, NewRetrospective.Handler((input, _) =>
                {
                    var newRetro = new RetrospectiveInstance
                        { RetrospectiveInstanceIdentifier = Guid.NewGuid(), Name = input.Name };
                    _retrospectives.Add(newRetro);
                    return newRetro.RetrospectiveInstanceIdentifier;
                }))
            )
            .Build();

        return await session.AcceptGatewayRequest(request);
    }
}