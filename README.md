# MythCrimeMapping

## Relocation

2025-02-13 the repository was moved from [Github: Jolo1Reper/MiphiCrimeMapping](https://github.com/Jolo1Reper/MiphiCrimeMapping). All the history of changes up to this time is stored there.

## Annotation

The application provides the ability to display and manage crime information on the map. With its help, you can:

<li>Display data on committed crimes on the map.</li>

<li>Add, edit and delete information on crimes.</li>

<li>Filter data for convenient analysis.</li>

<li>Manage the list of crime types.</li>

<li>Manage criminal data.</li>

<li>View visualization of statistics by crime types and by criminals.</li>

The application is designed for convenient analysis and management of crime data, providing a powerful tool for monitoring and decision-making.


## How to run in Docker

Using local docker files -- go to the repository directory and write the command:

`docker-compose up`

or

(Is best.) Using Docker Hub Images with a loadbalancer to support multiple APIs -- go to the repository directory and write the command:

`docker compose -f .\docker-compose-with-load-images.yml up --scale api=<number_api>`

After launching, the client will be available at `http://localhost:80`, at `http://localhost:8080/swagger` you can test the api.

To remove containers, write the command:

`docker-compose down`
