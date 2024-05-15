using DiffCalculator.Model;

namespace MailBusinessLayer.Facades.MailFacade;

public interface IMailFacade
{
    public Task Send(string fundName, DateOnly date);
}