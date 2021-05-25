# Crypto Portfolio Tracker
A tracker of your crypto portfolio, written in C# using .NET core. Made as KIV/NET semester project at Západočeská univerzita v Plzni.

- what's working:
    - most of the data layer implemented via SQLite database
        - integration tests ready
        - wrapper over SqlKata library
    - CoinGecko datasource implemented
        - integration tests ready
  - Blazor app startup with DI
    - fetching basic cryptocurrency stats via CG datasource and showing it on the "Fetch Data" page
## Electron
```electronize start /PublishSingleFile false /PublishReadyToRun false --no-self-contained```
  