export class Errors {
    data: Error[];
    message: Message[];
}

export interface Message {
    Code: string;
    Message: string;
    Path: string;
}

export interface Error {
    error: string;
}


