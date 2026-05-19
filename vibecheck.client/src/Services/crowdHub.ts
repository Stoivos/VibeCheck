import * as signalR from "@microsoft/signalr";

export const connection = new signalR.HubConnectionBuilder()
    .withUrl("/crowdhub")
    .withAutomaticReconnect()
    .build();