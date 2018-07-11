export class Authorizations {
    data: Authorization[];
    message: Message[];
}

export interface Message {
    Code: string;
    Message: string;
    Path: string;
}

export interface Authorization {
    jwt: string;
}


