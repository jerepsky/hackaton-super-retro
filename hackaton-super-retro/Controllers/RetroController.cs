using FeedbackBros.SuperRetro.SuperRetro.Retrospective;
using hackaton_super_retro.Builders;
using hackaton_super_retro.Models;
using Microsoft.AspNetCore.Mvc;
using Uniscale.Core;
using Uniscale.Designtime;

namespace hackaton_super_retro.Controllers;

[ApiController]
[Route("[controller]")]
public class RetroController : ControllerBase
{
    private readonly List<RetrospectiveData> _retrospectives;
    private readonly List<User> _users;
    private readonly ILogger<RetroController> _logger;

    public RetroController(ILogger<RetroController> logger)
    {
        _logger = logger;
        _retrospectives = new List<RetrospectiveData>();
    }

    [HttpPost(Name = "Request")]
    public async Task<Result> Post([FromBody] GatewayRequest request)
    {
        var session = await Platform.Builder()
            .WithInterceptors(i =>
                {
                    ManageRetrospective.Setup(i, _retrospectives);
                    ManageUser.Setup(i, _users);
                    
                    RetrospectiveAction.Setup(i, _retrospectives);
                    RetrospectiveAttendee.Setup(i, _retrospectives, _users);
                    RetrospectiveFeedback.Setup(i, _retrospectives);
                    RetrospectiveImprovementIdea.Setup(i, _retrospectives);
                    
                    RetrospectiveRead.Setup(i, _retrospectives, _users);
                }
        
            )
            .Build();
        
        return await session.AcceptGatewayRequest(request);
    }
}