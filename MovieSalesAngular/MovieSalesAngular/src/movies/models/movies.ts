export class Movies {
    data: Movie[];
    message: Message[];
  }
  
  interface Message {
    Code: number;
    Message: string;
    Path: string;
  }
  
  interface Movie {
    movieid: number;
    moviename: string;
    price: number;
    theaterreleasedate: Date;
    discreleasedate: Date;
    mpaarating: string;
    imageurl: string;
    movielength: string;
    lastmodified: Date;
    modifiedby: string;
  }
