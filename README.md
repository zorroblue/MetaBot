# MetaBot
> Virtual assistant chat bot for IIT Kharagpur

A chat bot for IIT Kharagpur, soon to be deployed of facebook messenger in the form of a public page. Users can message the page
and the chat bot will analyse and produce appropriate responses to their querries. Some sample queries and their generated responses are:
- What's new ?  : Will provide top latest news from four channels namely awaaz, scholar's avenue, IIT Kgp Tech and Gymkhana based
on the user's feedback in the prompt generated.
- Places to eat open now ?  : Will provide with a list of restaurants with available information regarding their timings and contact information
which are probably open now.
- Course info for MA10001 ? : Will provide grade distribution for the course with number of grades received by students in previous semesters.
for course MA10001.
- How to setup proxy ? : Will provide detailed instruction on how to setup proxy for using internet from whithin the institute

## Tools

- Lis for parsing the queries and extracting intents using NLP based ML models - https://www.luis.ai/
- Visual Studio 2015 with bot framework template
- Azure cloud services for deploying our bot- https://azure.microsoft.com/en-in/
- Heroku for deploying our scraping scripts- https://dashboard.heroku.com/

## Description

Most of our data source is [Meta Kgp](https://wiki.metakgp.org/w/Main_Page) - a media wiki for IIT Kgp maintained by the students of IIT Kgp.
The front-end is made in visual studio using bot framework template integrated with LUIS for NLP functionalities. Once the querry is parsed and
the underlying intent of the querry understood, the bot extracts information from appropriate sources most of which is maintained by
our python script deployed in Heroku cloud server which updates the cache every 15-30 minutes using REST APIs to provide real time data.

The bot is in development stage with much of feature integration to be done. Some future features to be integrated are professor's information.
previous year's paper for specific courses. If you have any more interesting features in mind, please add it in the issues.


## License

[![CC0](https://licensebuttons.net/p/zero/1.0/88x31.png)](https://creativecommons.org/publicdomain/zero/1.0/)

