﻿using Spectre.Console;

using Phonebook.MartinL_no.Models;
using Phonebook.MartinL_no.Controllers;
using Phonebook.MartinL_no.UserInterfaces;

namespace Phonebook.MartinL_no.Services;

internal static class ContactService
{
    public static void AddContact()
	{
        var name = AnsiConsole.Ask<string>("Contact's name: ");
        var phoneNumber = AnsiConsole.Ask<string>("Phone number: ");
        var email = AnsiConsole.Ask<string>("Email address: ");
        var type = GetContactType();

        ContactController.AddContact(new Contact { Name = name, PhoneNumber = phoneNumber, Email = email, Type = type });
    }

    public static void DeleteContact()
	{
		var contact = GetContactOptionInput();
		ContactController.DeleteContact(contact);
	}

    public static void GetContacts()
    {
        var contacts = ContactController.GetContacts();
        UserInterface.ShowContacts(contacts);
    }

    public static void GetContact()
    {
        var contact = GetContactOptionInput();
		UserInterface.ShowContact(contact);
    }

    public static void GetContactsByType()
    {
        var category = GetContactType();
        var contacts = ContactController.GetContacts().Where(x => x.Type == category).ToList();
        UserInterface.ShowContacts(contacts);
    }

    public static void UpdateContact()
    {
        var contact = GetContactOptionInput();
        contact.Name = AnsiConsole.Ask<string>("Contact's new name: ");
        contact.PhoneNumber = AnsiConsole.Ask<string>("Contact's new phone number: ");
        contact.Email = AnsiConsole.Ask<string>("Email address: ");
        contact.Type = GetContactType();

        ContactController.UpdateContact(contact);
    }

    public static async Task SendEmail()
    {
        var contact = GetContactOptionInput();
        var subject = AnsiConsole.Ask<string>("Subject: ");
        var content = AnsiConsole.Ask<string>("Content: ");

        await EmailController.SendEmail(contact, subject, content);
    }

    private static Contact GetContactOptionInput()
	{
		var contacts = ContactController.GetContacts();
		var contactsArray = contacts.Select(x => x.Name).ToArray();
		var option = AnsiConsole.Prompt(new SelectionPrompt<string>()
			.Title("Choose Contact")
			.AddChoices(contactsArray));
		var id = contacts.Single(x => x.Name == option).Id;
		var contact = ContactController.GetContactById(id);

		return contact;
	}

    private static ContactType GetContactType()
    {
        return AnsiConsole.Prompt(
        new SelectionPrompt<ContactType>()
        .Title("Choose contact type:")
        .AddChoices(
            ContactType.Family,
            ContactType.Friends,
            ContactType.Work,
            ContactType.None));
    }
}
