using FeedbackBros.SuperRetro.SuperRetro.Retrospective;
using HackathonSuperRetro.Interceptors;
using HackathonSuperRetro.Models;
using Microsoft.AspNetCore.Mvc;
using Uniscale.Core;
using Uniscale.Designtime;

namespace HackathonSuperRetro.Controllers;

[ApiController]
[Route("[controller]")]
public class RetroController : ControllerBase
{
    private readonly List<RetrospectiveData> _retrospectives;
    private readonly List<User> _users;
    private readonly ILogger<RetroController> _logger;

    public RetroController(ILogger<RetroController> logger, List<RetrospectiveData> retrospectives, List<User> users)
    {
        _logger = logger;
        _retrospectives = retrospectives;
        _users = users;
    }

    [HttpPost(Name = "Request")]
    public async Task<Result> Post([FromBody] GatewayRequest request)
    {
        var session = await Platform.Builder()
            .OnLogMessage(message => _logger.Log(message.Level.ToInternal(), message.Message))
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