export class UsersMovies {
    data: UsersMovie[];
    message: Message[];
  }

  interface Message {
    Code: number;
    Message: string;
    Path: string;
  }

  interface UsersMovie {
    movieid: number;
    userid: number;
    moviename: string;
    price: number;
    availableforpurchase: number;
    theaterreleasedate: Date;
    discreleasedate: Date;
    mpaarating: string;
    imageurl: string;
    movielength: string;
    datepurchased: Date;
    lastmodified: Date;
    modifiedby: string;
  }
