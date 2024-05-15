using DiffCalculator.Model;
using GenericBusinessLayer.Services;
using MailDataAccessLayer.Models;

namespace MailBusinessLayer.Services;

public interface IMailService
{
    Task<String>? SendEmail();
}