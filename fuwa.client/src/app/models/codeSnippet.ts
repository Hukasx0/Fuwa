import { PostedBy } from "./postedBy"

export class CodeSnippet {
    id: number = 0
    postedBy: PostedBy = new PostedBy
    title: string = ""
    description: string = ""
    code: string = ""
    mixedFrom: Mix = new Mix
    createdDate: Date = new Date
    lastModifiedDate: Date = new Date
    codeLanguage: number = 0
    likedBy: PostedBy[] = []
    mixes: Mix[] = []
}

class Mix {
    title: string = ""
    author: PostedBy = new PostedBy
    createdDate: Date = new Date
}