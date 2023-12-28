import { PostedBy } from "./postedBy"

export class CodeSnippetShort {
    id: number = 0
    postedBy: PostedBy = new PostedBy
    title: string = ""
    description: string = ""
    mixedFrom: Mix = new Mix
    createdDate: Date = new Date
    lastModifiedDate: Date | undefined
    codeLanguage: number = 0
}

class Mix {
    title: string = ""
    authorTag: string = ""
}
