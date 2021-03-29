# Sensate IoT - Smart Energy

The Sensate IoT Smart Energy project implements an IoT solution for (Dutch)
Smart Meters. The project consists of several repository's:

- DSMR parser;
- DSMR web client (implementator of this service);
- DSMR processor;
- DSMR API (this project).

## DSMR API

The DSMR API is the central customer facing API of the DSMR project. This API is
responsible for:

- Customer onboarding;
- Data point lookups from the OLAP;
- Setting up new customers in backup services.

End-users use their product token to authenticate with the DSMR API.
