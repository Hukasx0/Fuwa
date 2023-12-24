import { PostedBy } from "./postedBy"

export class PostComment {
    id: number = 0
    postedBy: PostedBy = new PostedBy
    text: string = ""
    createdDate: Date = new Date()
    lastModifiedDate: Date | undefined
}