import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { EntriesService } from '../../services/entries.service';
import { Entry } from '../../models/entry.model';
import { PostCardComponent } from '../../shared/components/post-card/post-card.component';
import { SharedModule } from '../../shared/shared.module';

@Component({
  selector: 'app-author-posts',
  standalone: true,
  imports: [SharedModule],
  templateUrl: './author-posts.component.html',
  styleUrl: './author-posts.component.css'
})
export class AuthorPostsComponent {
  author: string = '';
  posts: Entry[] = [];

  constructor(private route: ActivatedRoute, private entriesService: EntriesService) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.author = params.get('author') || '';
      this.loadPostsByAuthor(this.author);
    });
  }

  loadPostsByAuthor(author: string): void {
    this.entriesService.getEntries().subscribe(entries => {
      this.posts = entries.filter(entry => entry.author === author);
    });
  }
}

