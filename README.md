# Carepatron Backend 
CRUD backend Api application for Carepatron. 

1. Create a client 
2. Edit a client
3. Search for a client using first and last names
4. Emits events upon client creation. 

## Prerequisites 
Before starting, please install the following:
1. Install Visual Studio 2019 or 2022 in your machine.
2. Install .NetCore 6.x

Unit Tests setup:
1. Add XUnit package

## Getting Started
1. Clone the repo into a new directory: git clone git@github
2. Open Visual Studio and select IIS Express debug profile

## Running the tests
1. Open Visual Studio 
2. Open Test Explorer. Test > Test Explorer
3. Press the Run Button or Right click on the test header and select Run

## Events
The backend application emits events on creating new Clients. 
To subscribe to the event publisher, create a new subscriber class and subscribe to ClientEventPublisher. 

The current event subscriber stores the new client event data to a mock list. 

```
public ClientEventSubscriber(ClientEvent clientEvent, ClientEventPublisher clientEventPublisher)
{
    _clientEvent = clientEvent;

    clientEventPublisher.RaiseClientEvent += StoreNewClientInformationEvent;
}

private void StoreNewClientInformationEvent(object sender, ClientEventArgs e)
{
    ClientDataEvent.ClientEventList.Add(e.ClientEvent);
}
```

The client event consists of the following fields: 

```
public class ClientEvent
{
    public Guid Id { get; set; }
    public string ClientId { get; set; } = "";
    public string ClientFirstName { get; set; } = "";
    public string ClientLastName { get; set; } = "";    
    public DateTime DateCreated { get; set; }
}
```

## Built With
1. .Net Core 6
2. C#
3. XUnit for unit testing

## Authors
* **Joel Carlo Menor** 

## Acknowledgments
* Hat tip to Carepatron team for this fun and challenging task.
