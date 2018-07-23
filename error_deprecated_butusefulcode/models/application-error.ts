export class ApplicationErrors {
    data: ApplicationError[];
    message: Message[];
}

export interface Message {
    Code: string;
    Message: string;
    Path: string;
}

export interface ApplicationError {
    error: string;
}


