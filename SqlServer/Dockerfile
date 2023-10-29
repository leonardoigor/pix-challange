FROM mcr.microsoft.com/mssql/server:2019-latest

WORKDIR /usr/src/app
COPY . /usr/src/app/

ENV SA_PASSWORD SqlServer2019!
ENV ACCEPT_EULA Y
ENV MSSQL_PID Developer
ENV MSSQL_DB pix


EXPOSE 1433
RUN echo "teste"

CMD /bin/bash ./entrypoint.sh
