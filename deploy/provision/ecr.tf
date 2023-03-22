resource "aws_ecr_repository" "api" {
  name = "api"
}

resource "aws_ecr_repository" "k6" {
  name = "k6"
}

output "api_ecr_repository_url" {
  value = aws_ecr_repository.api.repository_url
}

output "k6_ecr_repository_url" {
  value = aws_ecr_repository.k6.repository_url
}
