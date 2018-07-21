export class TokenResponses {
    data: TokenResponse[];
    message: Message[];
}

export interface Message {
    Code: string;
    Message: string;
    Path: string;
}

export interface TokenResponse {
    token: string;
    expiration: string;
    username: string;

}


